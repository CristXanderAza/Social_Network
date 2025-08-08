using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.DTOs.Battleship.BattleshipGames
{
    public class GameHistoryDTO
    {
        public required Guid GameId { get; set; }
        public required string Winner {  get; set; }
        public required bool Won { get; set; }
        public required DateTime StartedAt { get; set; }
        public required DateTime FinishedAt { get; set; }
    }
}
