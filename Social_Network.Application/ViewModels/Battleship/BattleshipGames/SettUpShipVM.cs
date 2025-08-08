using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Battleship.BattleshipGames
{
    public class SettUpShipVM
    {
        public Guid GameId { get; set; }
        public IEnumerable<int> PosibleSized {  get; set; }
        public int SelectedSize { get; set; }
    }
}
