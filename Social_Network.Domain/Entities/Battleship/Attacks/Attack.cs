using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Battleship.BattleshipGames;
using Social_Network.Core.Domain.Entities.Battleship.Ships;

namespace Social_Network.Core.Domain.Entities.Battleship.Attacks
{
    public class Attack : BaseEntity<Guid>
    {
        public Guid BattleShipGameId { get; set; }
        public BattleShipGame BattleShipGame { get; set; }
        public string PlayerId { get; set; }
       // public DomainUser Player { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public AttackResult Result { get; set; } 
        public Guid? ShipId { get; set; }
        public Ship? Ship { get; set; }
    }
}
