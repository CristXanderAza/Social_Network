using Social_Network.Core.Domain.Entities.Battleship.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories.Battleship.Ships
{
    public interface IShipRepository : IRepositoryBase<Ship, Guid>
    {
        Task<bool> IsFreeToPutInRow(Guid gameId, string userId, int y, IEnumerable<int> xs);
        Task<bool> IsFreeToPutInColumn(Guid gameId, string userId, int x, IEnumerable<int> ys);
        Task<Ship> GetShipWithAttacksInPosition(int x, int y, Guid gameId, string userId);
        Task<int> GetCountOfEnemyShipsNonSunk(string userId, Guid gameId);

        Task<int> GetCountOfUserShips(string userId, Guid gameId);
        Task<int> GetCountOfTotalShips(Guid gameId);
    }
}
