using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Interfaces.Repositories.Battleship.Ships;
using Social_Network.Core.Domain.Entities.Battleship.Ships;
using Social_Network.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infraestructure.Persistence.Repositories.Battleship.Ships
{
    public class ShipRepository : BaseRepository<Ship, Guid>, IShipRepository
    {
        public ShipRepository(SocialNetworkContext context) : base(context)
        {
        }

        public async Task<int> GetCountOfEnemyShipsNonSunk(string userId, Guid gameId)
            => await _context.Ships
                        .Where(s => (!s.IsSunk) && s.PlayerId != userId && s.BattleShipGameId == gameId)
                        .CountAsync();

        public Task<int> GetCountOfTotalShips(Guid gameId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountOfUserShips(string userId, Guid gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<Ship> GetShipWithAttacksInPosition(int x, int y, Guid gameId, string userId)
            => await _context.Ships
                        .Where(s => s.BattleShipGameId == gameId && s.PlayerId != userId)
                        .Where(s => s.ShipPositions.Any(sp => sp.X == x && sp.Y == y))
                        .Include(s => s.Attacks)
                        .FirstOrDefaultAsync();

        public async Task<bool> IsFreeToPutInColumn(Guid gameId, string userId, int x, IEnumerable<int> ys)
            => !(await _context.Ships
                        .Where(s => s.BattleShipGameId == gameId && s.PlayerId == userId)
                        .Where(s => s.ShipPositions.Any(sp => sp.X == x && ys.Contains(sp.Y)))
                        .AnyAsync());

        public async Task<bool> IsFreeToPutInRow(Guid gameId, string userId, int y, IEnumerable<int> xs)
                     => !(await _context.Ships
                        .Where(s => s.BattleShipGameId == gameId && s.PlayerId == userId)
                        .Where(s => s.ShipPositions.Any(sp => sp.Y == y && xs.Contains(sp.X)))
                        .AnyAsync());
    }
}
