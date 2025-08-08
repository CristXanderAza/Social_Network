using Social_Network.Core.Application.DTOs.Battleship.BattleshipGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.DTOs.Battleship.Ships
{
    public class PutShipInPositionDto
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public int PositionOptionId { get; set; }
        public Guid GamId { get; set; }
        public string UserId { get; set; }
    }
}
