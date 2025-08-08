using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Interfaces.Repositories.Battleship.BattleshipGames;
using Social_Network.Core.Domain.Entities.Battleship.BattleshipGames;
using Social_Network.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infraestructure.Persistence.Repositories.Battleship.BattleshipGames
{
    public class BattleShipGameRepository : BaseRepository<BattleShipGame, Guid>, IBattleShipGameRepository
    {
        public BattleShipGameRepository(SocialNetworkContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BattleShipGame>> GetFinishedGamesOf(string userId)
            => await _context.BattleShipGames
                        .Where(bg => bg.Status == BattleshipGameStatus.Finished)
                        .Where(bg => bg.Player1Id == userId || bg.Player2Id == userId)
                        .ToListAsync();

        public async Task<IEnumerable<BattleShipGame>> GetCurrentGamesOf(string userId)
                        => await _context.BattleShipGames
                        .Where(bg => bg.Status != BattleshipGameStatus.Finished)
                        .Where(bg => bg.Player1Id == userId || bg.Player2Id == userId)
                        .ToListAsync();
    }
}
