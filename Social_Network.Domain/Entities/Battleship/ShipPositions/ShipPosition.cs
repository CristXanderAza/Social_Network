using Social_Network.Core.Domain.Entities.Battleship.Ships;

namespace Social_Network.Core.Domain.Entities.Battleship.ShipPositions
{
    public class ShipPosition
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ShipId { get; set; }
        public Ship Ship { get; set; } 
        public int X { get; set; } 
        public int Y { get; set; } 

    }
}
