using Social_Network.Core.Application.DTOs.Battleship.BattleshipGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Battleship.BattleshipGames
{
    public class CurrentGameInfoVM
    {
        public int Fase { get; set; }
        public bool CanAction { get; set; }
        public string Message { get; set; }
        public int? Size { get; set; } = 0;
        public Guid GameId { get; set; }
        public IEnumerable<LockedCellDto> LockedCells { get; set; } = Enumerable.Empty<LockedCellDto>();
    }
}
