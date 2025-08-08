using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.DTOs.Battleship.BattleshipGames
{
    public class LockedCellDto
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public LockedType Type { get; set; }
    }
}
