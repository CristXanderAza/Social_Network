using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.DTOs.Battleship.Attacks;
using Social_Network.Core.Application.DTOs.Battleship.BattleshipGames;
using Social_Network.Core.Application.DTOs.Battleship.Ships;
using Social_Network.Core.Application.DTOs.Users;
using Social_Network.Core.Application.Interfaces.Repositories.Battleship.Attacks;
using Social_Network.Core.Application.Interfaces.Repositories.Battleship.BattleshipGames;
using Social_Network.Core.Application.Interfaces.Repositories.Battleship.Ships;
using Social_Network.Core.Application.Interfaces.Repositories.Friendships;
using Social_Network.Core.Application.Interfaces.Services.Battleship;
using Social_Network.Core.Application.Interfaces.Services.Users;
using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Battleship.Attacks;
using Social_Network.Core.Domain.Entities.Battleship.BattleshipGames;
using Social_Network.Core.Domain.Entities.Battleship.ShipPositions;
using Social_Network.Core.Domain.Entities.Battleship.Ships;

namespace Social_Network.Core.Application.Services.Battleship
{
    public class BattleShipService : IBattleShipService
    {
        private readonly IBattleShipGameRepository _battleShipGameRepository;
        private readonly IFriendshipRepository _friendshipRepository; 
        private readonly IUserService _userService;
        private readonly IAttackRepository _attackRepository;
        private readonly IShipRepository _shipRepository;
        private readonly IMapper _mapper;

        public BattleShipService(IBattleShipGameRepository battleShipGameRepository, IAttackRepository attackRepository,
            IShipRepository shipRepository, IMapper mapper, IUserService userService, IFriendshipRepository friendshipRepository)
        {
            _battleShipGameRepository = battleShipGameRepository;
            _attackRepository = attackRepository;
            _shipRepository = shipRepository;
            _mapper = mapper;
            _userService = userService;
            _friendshipRepository = friendshipRepository;
        }

        public async Task<Result<Unit>> AttackOnPosition(AttackOnPositionDTO dTO)
        {
            var gameRes = await _battleShipGameRepository.GetById(dTO.GameId);
            if(gameRes.IsFailure)
            {
                return Result<Unit>.Fail("Game not found");
            }
            var game = gameRes.Value;
            if(game.Status != BattleshipGameStatus.InProgress)
            {
                return Result<Unit>.Fail("You cannot attack in this moment, is not InProcess Fase");
            }
            if(game.CurrentTurnPlayerId != dTO.UserId)
            {
                return Result<Unit>.Fail("You cannot attack in this moment, is not your turn");
            }
            bool isAnyAttackInPosition = await _attackRepository.Any(a => a.BattleShipGameId == game.Id && a.PlayerId == dTO.UserId 
                                                                        && a.X == dTO.X && a.Y == dTO.Y);
            if (isAnyAttackInPosition)
                return Result<Unit>.Fail("You have already attack here");
            Attack attack = new Attack
            {
                X = dTO.X,
                Y = dTO.Y,
                CreatedAt = DateTime.UtcNow,
                BattleShipGameId = game.Id,
                PlayerId = dTO.UserId,
            };
            Ship? posibleShipAttacked = await _shipRepository.GetShipWithAttacksInPosition(attack.X, attack.Y, game.Id, dTO.UserId);
            int? previosAttackCount = null; 
            if (posibleShipAttacked != null)
            {
                previosAttackCount = posibleShipAttacked.Attacks.Count;
                attack.ShipId = posibleShipAttacked.Id;
                attack.Result = AttackResult.Hit;
            }
            var saveRes = await _attackRepository.AddAsync(attack);
            if (saveRes.IsFailure)
                return Result<Unit>.Fail(saveRes.Error);
            if (previosAttackCount.HasValue)
            {
                posibleShipAttacked.IsSunk = (previosAttackCount + 1 == posibleShipAttacked.ShipSize);
                await _shipRepository.UpdateAsync(posibleShipAttacked);
            }
            if(await _checkIfWon(game.Id, dTO.UserId))
            {
                game.WinnerId = dTO.UserId;
                game.Status = BattleshipGameStatus.Finished;
                game.FinishedAt = DateTime.UtcNow;
                await _battleShipGameRepository.UpdateAsync(game);
            }
            else
            {
                game.CurrentTurnPlayerId = (dTO.UserId == game.Player1Id)? game.Player2Id : game.Player1Id;
                game.LastMovementAt = DateTime.UtcNow;
                await _battleShipGameRepository.UpdateAsync(game);
            }
            return Unit.Value;
        }

        public async Task<Result<Guid>> CreateNewBattleShipGame(BattleShipGameInsertDTO insertDTO)
        {
            var isAnyGameCurrently = await _battleShipGameRepository.Any(bg 
                => bg.Status !=BattleshipGameStatus.Finished && 
                ((bg.Player1Id == insertDTO.Player1Id && bg.Player2Id == insertDTO.Player2Id) 
                || (bg.Player1Id == insertDTO.Player2Id && bg.Player2Id == insertDTO.Player1Id)));
            if (isAnyGameCurrently)
            {
                return Result<Guid>.Fail("There is already a game between these players");
            }
            var game = new BattleShipGame
            {
                Player1Id = insertDTO.Player1Id,
                Player2Id = insertDTO.Player2Id,
                CurrentTurnPlayerId = insertDTO.Player1Id,
                Status = BattleshipGameStatus.SettingUpShips,
                StartedAt = DateTime.UtcNow,
            };
            var res = await _battleShipGameRepository.AddAsync(game);
            if (res.IsFailure)
            {
                return Result<Guid>.Fail(res.Error.ToString());
            }
            return res.Value.Id;
        }

        public async Task<Result<BattleShipGamesResumeDTO>> GerResumeOfUser(string userId)
            => await _battleShipGameRepository.AsQuery()
                .Where(bg =>bg.Status == BattleshipGameStatus.Finished && (bg.Player1Id == userId || bg.Player2Id == userId))
                .GroupBy(g => 1)
                .Select(G => new BattleShipGamesResumeDTO
                {
                    GamesPlayed = G.Count(),
                    Won = G.Where(bg => bg.WinnerId == userId).Count(),
                    Lose = G.Where(bg => bg.WinnerId != userId).Count(),
                }).FirstOrDefaultAsync();

        public async Task<IEnumerable<BattleShipGameDTO>> GetGames(string userId)
        {
            var games = await _battleShipGameRepository.GetCurrentGamesOf(userId);
            var gamesIds1 = games.Where(g => g.Player1Id == userId).Select(g => g.Player2Id);
            var gamesIds2 = games.Where(g => g.Player2Id == userId).Select(g => g.Player1Id);
            var headers = await _userService.GetHeadersOfUsers(gamesIds2.Concat(gamesIds1));
            return games.Select(g =>
            {
                var anotherUserId = g.Player1Id == userId? g.Player2Id : g.Player1Id;
                return new BattleShipGameDTO
                {
                    Id = g.Id,
                    Status = g.Status.ToString(),
                    DurationInHours = (DateTime.UtcNow - g.StartedAt).TotalHours,
                    OponentName = headers[anotherUserId].UserName,
                };
            });
        }

        
        public async Task<Result<Unit>> GiveUp(string userId, Guid gameId)
        {

            var gameres = await _battleShipGameRepository.GetById(gameId);
            if (gameres.IsFailure)
            {
                return Result<Unit>.Fail(gameres.Error);
            }
            var game = gameres.Value;
            if(game.Status == BattleshipGameStatus.Finished)
            {
                return Result<Unit>.Fail("tHE GAME IS OVER");
            }
            if(game.Player1Id == userId || game.Player2Id == userId)
            {
                var otherId = game.Player1Id == userId ? game.Player2Id : game.Player1Id;
                game.WinnerId = otherId;
                game.Status = BattleshipGameStatus.Finished;
                game.FinishedAt = DateTime.UtcNow;
                var res = await _battleShipGameRepository.UpdateAsync(game);
                if (res.IsSuccess)
                {
                    return Unit.Value;
                }
                else
                {
                    return Result<Unit>.Fail(res.Error);
                }


            }
            return Result<Unit>.Fail("You are not part of this game");

        }

        


        public async Task<Result<GameCurrentInfoDTO>> GetCurrentGame(Guid GameId, string userId)
        {
            
            var gameRes = await _battleShipGameRepository.GetById(GameId);
            if (gameRes.IsFailure)
            {
                return Result<GameCurrentInfoDTO>.Fail("Game not found");
            }
            var game = gameRes.Value;
            if (game.Player1Id != userId && game.Player2Id != userId)
            {
                return Result<GameCurrentInfoDTO>.Fail("You are not part of this game");
            }

            if ((DateTime.UtcNow - game.LastMovementAt) > TimeSpan.FromDays(2))
            {
                var winnerId = game.CurrentTurnPlayerId == game.Player1Id ? game.Player2Id : game.Player1Id;
                game.WinnerId = winnerId;
                game.Status = BattleshipGameStatus.Finished;
                game.FinishedAt = DateTime.UtcNow;
                await _battleShipGameRepository.UpdateAsync(game);
                return Result<GameCurrentInfoDTO>.Fail("Game expired. Winner declared by inactivity");
            }
            IEnumerable<LockedCellDto> lockedCellDtos;
            if(game.Status == BattleshipGameStatus.SettingUpShips)
            {
                lockedCellDtos = await _shipRepository.AsQuery()
                                  .Where(s => s.PlayerId == userId && s.BattleShipGameId == GameId)
                                  .SelectMany(s => s.ShipPositions)
                                  .Select(s => new LockedCellDto
                                  {
                                      Type = LockedType.Using,
                                      X = s.X,
                                      Y = s.Y,
                                  }).ToListAsync();
            }
            else
            {
                lockedCellDtos = await _attackRepository.AsQuery()
                                  .Where(a => a.PlayerId == userId && a.BattleShipGameId == GameId)
                                  .Select(a => new LockedCellDto
                                  {
                                      Type = a.Ship != null ? LockedType.Hited : LockedType.Missed,
                                      X = a.X,
                                      Y = a.Y,
                                  }).ToListAsync();
            }
            string? WinnerName = null;
            if(game.Status == BattleshipGameStatus.Finished)
            {
                var header = await _userService.GetHeaderOfUsers(game.WinnerId);
                WinnerName = header.UserName;
            }
            

            return new GameCurrentInfoDTO
            {
                LockedCells = lockedCellDtos,
                Winner = WinnerName,
                Fase = (int)game.Status,
                CanAction = game.Status == BattleshipGameStatus.SettingUpShips? true : (game.CurrentTurnPlayerId == userId)
            };
        }

        public async Task<IEnumerable<PositionOption>> GetPosibleMovementsOn(GetPosibleMovementOptionsOnDTO dTO)
        {
            int normalizedSize = dTO.Size - 1;
            List<PositionOption> movements = new();
            if(dTO.X - normalizedSize > 0)
            {
                movements.Add(PositionOption.Left);
            }
            if(dTO.X + normalizedSize < 13)
            {
                movements.Add(PositionOption.Right);
            }
            if (dTO.Y - normalizedSize > 0)
            {
                movements.Add(PositionOption.Up);
            }
            if (dTO.Y + normalizedSize < 13)
            {
                movements.Add(PositionOption.Down);
            }
            return movements;
        }

        public async Task<Result<IEnumerable<LockedCellDto>>> GetResultsOfOponent(Guid GameId, string userId)
        {
            var gameRes = await _battleShipGameRepository.GetById(GameId);
            if (gameRes.IsFailure)
                return Result<IEnumerable<LockedCellDto>>.Fail("Game not found");
            BattleShipGame game = gameRes.Value;
            if (game.Status != BattleshipGameStatus.Finished)
                return Result<IEnumerable<LockedCellDto>>.Fail("The game is not finishet yet, you cannot see the results");
            return await _attackRepository.AsQuery()
                .Where(a => a.PlayerId != userId && a.BattleShipGameId == game.Id)
                .Select(a => new LockedCellDto
                {
                    Type = (a.Result == AttackResult.Hit) ? LockedType.Hited : LockedType.Missed,
                    X = a.X,
                    Y = a.Y,
                }).ToListAsync();
        }

        public async Task<Result<IEnumerable<LockedCellDto>>> GetResultsOfPlayer(Guid GameId, string userId)
        {
            var gameRes = await _battleShipGameRepository.GetById(GameId);
            if (gameRes.IsFailure)
                return Result<IEnumerable<LockedCellDto>>.Fail("Game not found");
            BattleShipGame game = gameRes.Value;
            if(game.Status != BattleshipGameStatus.Finished)
                return Result<IEnumerable<LockedCellDto>>.Fail("The game is not finishet yet, you cannot see the results");
            return await _attackRepository.AsQuery()
                .Where(a => a.PlayerId == userId && a.BattleShipGameId == game.Id)
                .Select(a => new LockedCellDto
                {
                    Type = (a.Result == AttackResult.Hit) ? LockedType.Hited : LockedType.Missed,
                    X = a.X,
                    Y = a.Y,
                }).ToListAsync();
        }

        public async Task<Result<IEnumerable<LockedCellDto>>> GetResultsOfriginalPositions(Guid GameId, string userId)
        {
            var gameRes = await _battleShipGameRepository.GetById(GameId);
            if (gameRes.IsFailure)
                return Result<IEnumerable<LockedCellDto>>.Fail("Game not found");
            BattleShipGame game = gameRes.Value;
            if (game.Status != BattleshipGameStatus.Finished)
                return Result<IEnumerable<LockedCellDto>>.Fail("The game is not finishet yet, you cannot see the results");
            return await _shipRepository.AsQuery()
                .Where(S => S.BattleShipGameId == game.Id && S.PlayerId == userId)
                .SelectMany(s=> s.ShipPositions)
                .Select(a => new LockedCellDto
                {
                    Type = LockedType.Using,
                    X = a.X,
                    Y = a.Y,
                }).ToListAsync();
        }

        public async Task<Result<Unit>> PutShipOnPosition(PutShipInPositionDto dto)
        {
            var gameRes = await _battleShipGameRepository.GetById(dto.GamId);
            if (gameRes.IsFailure)
            {
                return Result<Unit>.Fail("Game not found");
            }
            if(gameRes.Value.Player1Id != dto.UserId && gameRes.Value.Player2Id != dto.UserId)
            {
                return Result<Unit>.Fail("You are not authorized to act in this game");
            }
            var optionsDisp = await GetOptionToPutShip(dto.GamId, dto.UserId);
            if(!optionsDisp.Any(op => op.Size == dto.Size))
                return Result<Unit>.Fail("You cannot put a ship of this size");
            var option = (PositionOption)dto.PositionOptionId;
            bool isFree = false;
            IEnumerable<ShipPosition> positions = Enumerable.Empty<ShipPosition>(); 
            if(option  == PositionOption.Left || option == PositionOption.Right)
            {
                List<int> xs = new List<int>();
                for (int i = 0; i < dto.Size; i++)
                {
                    if(option == PositionOption.Right)
                    {
                        xs.Add(dto.X + i);
                    }
                    else
                    {
                        xs.Add(dto.X - i);
                    }
                }
                if(!xs.Any(n => n < 0 && n > 12))
                {
                    isFree = await _shipRepository.IsFreeToPutInRow(dto.GamId, dto.UserId, dto.Y, xs);
                    if(isFree) 
                        positions = ComputePositionOnRow(dto.Y, xs);
                }
                   
            }
            else if (option == PositionOption.Up || option == PositionOption.Down)
            {
                List<int> ys = new List<int>();
                for (int i = 0; i < dto.Size; i++)
                {
                    if (option == PositionOption.Up)
                    {
                        ys.Add(dto.Y - i);
                    }
                    else
                    {
                        ys.Add(dto.Y + i);
                    }
                }
                if (!ys.Any(n => n < 0 && n > 12))
                {
                    isFree = await _shipRepository.IsFreeToPutInColumn(dto.GamId,dto.UserId , dto.X, ys);
                    if (isFree)
                        positions = ComputePositionOnColumn(dto.X, ys);
                }
                    
            }
            if(isFree && positions.Any())
            {
                var ship = new Ship
                {
                    ShipSize = dto.Size,
                    PlayerId = dto.UserId,
                    ShipPositions = positions.ToList(),
                    BattleShipGameId = dto.GamId
                };
                var res = await _shipRepository.AddAsync(ship);
                if (res.IsFailure)
                {
                    return Result<Unit>.Fail(res.Error);
                }
                if (await AreAllShipsPuted(gameRes.Value.Id))
                {
                    var game = gameRes.Value;
                    game.Status = BattleshipGameStatus.InProgress;
                    await _battleShipGameRepository.UpdateAsync(game);
                }

                return Unit.Value;

            }
            return Result<Unit>.Fail("You cannot put this ship in these positions");
        }

        private IEnumerable<ShipPosition> ComputePositionOnRow(int y, IEnumerable<int> xs)
            => xs.Select(nx => new ShipPosition
            {
                Y = y,
                X = nx,
            }).ToArray();

        private IEnumerable<ShipPosition> ComputePositionOnColumn(int x, IEnumerable<int> ys)
            => ys.Select(ny => new ShipPosition
            {
                Y = ny,
                X = x,
            }).ToArray();

        private async Task<bool> _checkIfWon(Guid gameId, string userId)
        {
            int countOfEnemyShips = await _shipRepository.GetCountOfEnemyShipsNonSunk(userId, gameId);
            return countOfEnemyShips <= 0;
        }

        public async  Task<IEnumerable<GameHistoryDTO>> GetHistoriesFor(string userId)
        {
            var battles = await _battleShipGameRepository.GetFinishedGamesOf(userId);
            var winnersIdS = battles.Where(bg => bg.WinnerId != userId).Select(bg => bg.WinnerId).Distinct().ToList();
            var headers = await _userService.GetHeadersOfUsers(winnersIdS);
            return battles.Select(bg => new GameHistoryDTO
            {
                FinishedAt = bg.FinishedAt.Value,
                StartedAt = bg.StartedAt,
                GameId = bg.Id,
                Won = bg.WinnerId == userId,
                Winner = (bg.WinnerId == userId) ? "Yo" : headers[bg.WinnerId].UserName
            });
        }

        public async Task<IEnumerable<UserHeaderDTO>> GetPosibleUsersForNewParty(string userId)
        {
            var friendIds = await _friendshipRepository.GetFriendsId(userId);
            var activePartysIds = await _battleShipGameRepository.AsQuery()
                .Where(bg => bg.Status != BattleshipGameStatus.Finished)
                .Where(bg => bg.Player1Id == userId || bg.Player2Id == userId)
                .Select(bg => bg.Player1Id ==userId? bg.Player2Id : bg.Player2Id).ToListAsync();
            var headers = await _userService.GetHeadersOfUsers(friendIds.Except(activePartysIds).ToList());
            return friendIds.Select(id => headers[id]).ToList();    
        }

        public async Task<IEnumerable<ShipOption>> GetOptionToPutShip(Guid GameId, string userId)
        {
            var sizesPuted = await _shipRepository.AsQuery()
                .Where(s => s.PlayerId == userId && s.BattleShipGameId == GameId)
                .Select(s => s.ShipSize).ToListAsync();
            List<ShipOption> options = new List<ShipOption>();
            for(int i = 2; i <= 5; i++)
            {
                if(i == 3)
                {
                    for(int j = 0; j < 2 - sizesPuted.Where(n => n == 3).Count(); j++)
                    {
                        options.Add(new ShipOption { Size = i });
                    }
                }
                else
                {
                    if (!sizesPuted.Contains(i))
                        options.Add(new ShipOption { Size = i });
                }
            }
           return options;
        }
//lo

        private async Task<bool> AreAllShipsPuted(Guid GameId)
            => (await _shipRepository.AsQuery()
                       .Where(s => s.BattleShipGameId == GameId)
                      .CountAsync()) > 9;   
    }
}
