using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Battleship.Attacks;
using Social_Network.Core.Domain.Entities.Battleship.BattleshipGames;
using Social_Network.Core.Domain.Entities.Battleship.ShipPositions;

namespace Social_Network.Core.Domain.Entities.Battleship.Ships
{
    public class Ship : BaseEntity<Guid>
    {
        public Guid BattleShipGameId { get; set; }
        public BattleShipGame BattleShipGame { get; set; }
        public string PlayerId { get; set; }
        //public DomainUser Player { get; set; }
        public int ShipSize { get; set; }
        public bool IsSunk { get; set; } = false;
        public ICollection<Attack> Attacks { get; set; } = new List<Attack>();
        public ICollection<ShipPosition> ShipPositions { get; set; } = new List<ShipPosition>();
    }
}
