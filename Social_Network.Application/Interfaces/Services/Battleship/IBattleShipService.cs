using Social_Network.Core.Application.DTOs.Battleship.Attacks;
using Social_Network.Core.Application.DTOs.Battleship.BattleshipGames;
using Social_Network.Core.Application.DTOs.Battleship.Ships;
using Social_Network.Core.Application.DTOs.Users;
using Social_Network.Core.Domain.Base;

namespace Social_Network.Core.Application.Interfaces.Services.Battleship
{
    public interface IBattleShipService
    {
        Task<Result<Guid>> CreateNewBattleShipGame(BattleShipGameInsertDTO insertDTO);
        Task<Result<Unit>> GiveUp(string userId, Guid gameId);
        Task<IEnumerable<ShipOption>> GetOptionToPutShip(Guid GameId, string userId);
        Task<IEnumerable<BattleShipGameDTO>> GetGames(string userId);
        Task<Result<GameCurrentInfoDTO>> GetCurrentGame(Guid GameId, string userId);
        Task<IEnumerable<PositionOption>> GetPosibleMovementsOn(GetPosibleMovementOptionsOnDTO dTO); 
        Task<Result<Unit>> PutShipOnPosition(PutShipInPositionDto dto);
        Task<Result<Unit>> AttackOnPosition(AttackOnPositionDTO dTO);
        Task<Result<IEnumerable<LockedCellDto>>> GetResultsOfPlayer(Guid GameId,string userId);
        Task<Result<IEnumerable<LockedCellDto>>> GetResultsOfOponent(Guid GameId, string userId);
        Task<Result<IEnumerable<LockedCellDto>>> GetResultsOfriginalPositions(Guid GameId, string userId);
        Task<Result<BattleShipGamesResumeDTO>> GerResumeOfUser(string userId);
        Task<IEnumerable<GameHistoryDTO>> GetHistoriesFor(string userId);
        Task<IEnumerable<UserHeaderDTO>> GetPosibleUsersForNewParty(string userId);
    }
}
