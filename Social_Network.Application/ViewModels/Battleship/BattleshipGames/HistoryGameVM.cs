using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Battleship.BattleshipGames
{
    public class HistoryGameVM
    {
        public required Guid GameId { get; set; }
        public required string Winner { get; set; }
        public required bool Won { get; set; }
        public required DateTime StartedAt { get; set; }
        public required DateTime FinishedAt { get; set; }
    }
}
