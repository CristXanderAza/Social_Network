using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.DTOs.Battleship.Attacks;
using Social_Network.Core.Application.DTOs.Battleship.BattleshipGames;
using Social_Network.Core.Application.DTOs.Battleship.Ships;
using Social_Network.Core.Application.Helpers.Enums;
using Social_Network.Core.Application.Interfaces.Services.Battleship;
using Social_Network.Core.Application.ViewModels.Battleship.BattleshipGames;
using Social_Network.Core.Application.ViewModels.Common;
using Social_Network.Core.Application.ViewModels.Users;
using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Posts;
using System.Security.Claims;

namespace Social_Network.WebApp.Controllers.Battleship.BattleshipGames
{
    [Authorize]
    public class BattleShipGameController : Controller
    {
        private readonly IBattleShipService _battleShipService;
        private readonly IMapper _mapper;

        public BattleShipGameController(IBattleShipService battleShipService, IMapper mapper)
        {
            _battleShipService = battleShipService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string? errors = null, string? message = null)
        {
            ViewBag.Errors = errors; 
            ViewBag.Message = message;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var activeGames = await _battleShipService.GetGames(userId);
            var activeVm = activeGames.Select(ag => _mapper.Map<ActiveGameVM>(ag));
            var histories = await _battleShipService.GetHistoriesFor(userId);
            var historyVms = histories.Select(h => _mapper.Map<HistoryGameVM>(h));
            var resume = await _battleShipService.GerResumeOfUser(userId);
            var resumeVm = _mapper.Map<BattleShipResumeVM>(resume.Value);
            var mainVm = new MainBattleShipVM
            {
                ActiveGameVMs = activeVm,
                HistoryGameVMs = historyVms,
                BattleShipResumeVM = resumeVm,
            };
            return View(mainVm);
        }

        public async Task<IActionResult> NewGame()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userHeader = await _battleShipService.GetPosibleUsersForNewParty(userId);
            var uhVms = userHeader.Select(u => _mapper.Map<UserHeaderVM>(u));
            var newGamVm = new NewGameVM
            {
                userHeaderVMs = uhVms,
            };
            return View(newGamVm);  
        }

        [HttpPost]
        public async Task<IActionResult> NewGame(NewGameVM newGame)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res =  await _battleShipService.CreateNewBattleShipGame(new BattleShipGameInsertDTO
            {
                Player1Id = userId,
                Player2Id = newGame.selectedOponent
            });
            if (res.IsFailure)
            {
                return RedirectToAction("Index", new { errors = res.Error });
            }
            return RedirectToAction("LoadGame", new {gamId = res.Value});
        }

        public async Task<IActionResult> LoadGame(Guid gameId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Result<GameCurrentInfoDTO> res = await _battleShipService.GetCurrentGame(gameId, userId);
            if (res.IsFailure)
            {
                return RedirectToAction("Index", new { errors = res.Error });
            }
            var currentInfo = res.Value;
            var currentVm = _mapper.Map<CurrentGameInfoVM>(currentInfo);
            switch (currentInfo.Fase)
            {
                case 1:
                    var posibleSizes = await _battleShipService.GetOptionToPutShip(gameId, userId);
                    return View("SettingUp", new SettUpShipVM
                    {
                        PosibleSized = posibleSizes.Select(p => p.Size).ToList(),
                        GameId = gameId,
                    });
                case 3:
                    return RedirectToAction("Index", new { message = $"Partida Terminada. Ganador {currentInfo.Winner}" });
                default:
                    currentVm.Message =(currentVm.CanAction)? "AL ATAQUE!!!" : "Esperando a que el opnente realice su turno";
                    currentVm.GameId = gameId;
                    return View("Table", currentVm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SettUp(SettUpShipVM vm)
        {
            if(!ModelState.IsValid)
            {

            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _battleShipService.GetCurrentGame(vm.GameId, userId);
            if (res.IsFailure)
            {
                return RedirectToAction("Index", new { errors = res.Error });
            }
            var currentInfo = res.Value;
            var currentVm = _mapper.Map<CurrentGameInfoVM>(currentInfo);
            currentVm.Message = "Selecciona una celda para posicionar tu barco. Luego elegirás la dirección.";
            if (currentInfo.Fase == 1)
            {
                currentVm.Size = vm.SelectedSize;
                currentVm.GameId = vm.GameId;
            }
            return View("Table", currentVm);
        }

        [HttpGet]
        public async Task<IActionResult> SetDirectionForShip(int x, int y, int size ,Guid gameId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var positionOprion = await _battleShipService.GetPosibleMovementsOn(new GetPosibleMovementOptionsOnDTO
            {
                Size = size,
                UserId = userId,
                X = x,
                Y = y,
            });
            SetDirectionVM setDirection = new()
            {
                PosibleDirections = positionOprion.Select(o => new Ident<int> { Id = (int)o, Name = o.ToString() }),
                Size = size,
                GameId = gameId,
                X = x,
                Y = y,
            };
            return View(setDirection);
        }

        [HttpPost]
        public async Task<IActionResult> SetDirectionForShip(SetDirectionVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {

            }
            var dto = new PutShipInPositionDto
            {
                GamId = vm.GameId,
                PositionOptionId = vm.Direction,
                Size = vm.Size,
                UserId = userId,
                X = vm.X,
                Y = vm.Y,
            };
            var res = await _battleShipService.PutShipOnPosition(dto);
            if (res.IsSuccess)
            {
                return RedirectToAction("LoadGame", new { gameId = vm.GameId});
            }
            else
            {
                return RedirectToAction("Index", new { errors = res.Error });
            }
        }

        public async Task<IActionResult> AttackOnPosition(int x, int y, Guid gameId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = new AttackOnPositionDTO
            {
                GameId = gameId,
                X = x,
                Y = y,
                UserId = userId
            };
            var res = await _battleShipService.AttackOnPosition(dto);
            if (res.IsSuccess)
            {
                return RedirectToAction("LoadGame", new { gameId = gameId });
            }
            else
            {
                return RedirectToAction("Index", new { errors = res.Error });
            }
        }

        public async Task<IActionResult> SeeResultsOfUser(Guid gameId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _battleShipService.GetResultsOfPlayer(gameId, userId);
            if (res.IsSuccess)
            {
                var current = new CurrentGameInfoVM
                {
                    Fase = 3,
                    LockedCells = res.Value,
                    GameId = gameId,
                    Message = "Mira tus ataques",
                    CanAction = false,
                };
                return View("Table", current);
            }
            else
            {
                return RedirectToAction("Index", new { errors = res.Error });
            }

        }

        public async Task<IActionResult> GiveUp(Guid gameId)
        {
            var dangerAletVm = new GenericAlertVM
            {
                ResourceId = gameId,
                Message = "¿Estas seguro que deseas rendirte",
                Controller = "BattleShipGame",
                ActionDestination = "GiveUp"
            };
            return View("GenericAlert", dangerAletVm);
        }

        [HttpPost]
        public async Task<IActionResult> GiveUp(GenericAlertVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _battleShipService.GiveUp(userId, vm.ResourceId);
            return RedirectToAction("Index", new { errors = res.Error });
        }

        public async Task<IActionResult> SeeOriginalPosition(Guid gameId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _battleShipService.GetResultsOfriginalPositions(gameId, userId);
            if (res.IsSuccess)
            {
                var current = new CurrentGameInfoVM
                {
                    Fase = 3,
                    LockedCells = res.Value,
                    GameId = gameId,
                    Message = "Mira tus posiciones originales",
                    CanAction = false,
                };
                return View("Table", current);
            }
            else
            {
                return RedirectToAction("Index", new { errors = res.Error });
            }

        }

        public async Task<IActionResult> SeeResultsOfOponent(Guid gameId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _battleShipService.GetResultsOfOponent(gameId, userId);
            if (res.IsSuccess)
            {
                var current = new CurrentGameInfoVM
                {
                    Fase = 3,
                    Message = "Mira los ataques del oponente",
                    GameId = gameId,
                    LockedCells = res.Value
                };
                return View("Table", current);
            }
            else
            {
                return RedirectToAction("Index", new { errors = res.Error });
            }
        }

    }
}
