using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.DTOs.Battleship.BattleshipGames
{
    public class BattleShipGamesResumeDTO
    {
        public string UserId { get; set; }
        public int GamesPlayed { get; set; }
        public int Won {  get; set; }
        public int Lose { get; set; }
    }
}
