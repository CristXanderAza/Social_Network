using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Domain.Entities.Battleship.BattleshipGames
{
    public enum BattleshipGameStatus
    {
        SettingUpShips = 1,
        InProgress = 2,
        Finished = 3,
    }
}
