using Social_Network.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.DTOs.Battleship.BattleshipGames
{
    public class NewGameVM
    {
        public IEnumerable<UserHeaderVM> userHeaderVMs { get; set; }
        public string selectedOponent {  get; set; }
    }
}
