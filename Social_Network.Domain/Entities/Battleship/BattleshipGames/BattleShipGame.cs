using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Battleship.Attacks;
using Social_Network.Core.Domain.Entities.Battleship.Ships;

namespace Social_Network.Core.Domain.Entities.Battleship.BattleshipGames
{
    public class BattleShipGame : BaseEntity<Guid>
    {
        public string Player1Id { get; set; }
        //public DomainUser Player1 { get; set; }
        public string Player2Id { get; set; }
       // public DomainUser Player2 { get; set; }
        public BattleshipGameStatus Status { get; set; } = BattleshipGameStatus.SettingUpShips;
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinishedAt { get; set; } = null;
        public string CurrentTurnPlayerId { get; set; }
       // public DomainUser CurrentTurnPlayer { get; set; }
        public DateTime? LastMovementAt { get; set; }
        public string WinnerId { get; set; } 
        //public DomainUser Winner { get; set; }
        public ICollection<Ship> Ships { get; set; } = new List<Ship>();
        public ICollection<Attack> Attacks { get; set; } = new List<Attack>();
    }
}
