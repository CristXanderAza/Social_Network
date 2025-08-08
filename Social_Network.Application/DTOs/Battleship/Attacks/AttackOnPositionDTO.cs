using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.DTOs.Battleship.Attacks
{
    public class AttackOnPositionDTO
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public string UserId { get; set; }
        public Guid GameId { get; set; }
    }
}
