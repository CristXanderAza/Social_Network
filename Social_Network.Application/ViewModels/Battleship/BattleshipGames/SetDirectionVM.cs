using Social_Network.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Battleship.BattleshipGames
{
    public class SetDirectionVM
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public Guid GameId { get; set; }
        public IEnumerable<Ident<int>>? PosibleDirections { get; set; }

        [Required(ErrorMessage ="This field is required")]
        public int Direction { get; set; }
    }
}
