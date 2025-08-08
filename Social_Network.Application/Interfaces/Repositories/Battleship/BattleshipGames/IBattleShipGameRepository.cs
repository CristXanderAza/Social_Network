using Social_Network.Core.Domain.Entities.Battleship.BattleshipGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories.Battleship.BattleshipGames
{
    public interface IBattleShipGameRepository : IRepositoryBase<BattleShipGame,Guid>
    {
        Task<IEnumerable<BattleShipGame>> GetCurrentGamesOf(string userId);
        Task<IEnumerable<BattleShipGame>> GetFinishedGamesOf(string userId);
    }
}
