using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.DTOs.Battleship.BattleshipGames
{
    public class BattleShipGameDTO
    {
        public Guid Id { get; set; }
        public string OponentName { get; set; }
        public string Status { get; set; }
        public double DurationInHours { get; set; }
    }
}
