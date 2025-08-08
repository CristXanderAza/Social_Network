namespace Social_Network.Core.Application.DTOs.Battleship.BattleshipGames
{
    public class GameCurrentInfoDTO
    {
        public int Fase {  get; set; }
        public bool CanAction { get; set; }
        public string? Winner { get; set; }
        public IEnumerable<LockedCellDto> LockedCells { get; set; } = Enumerable.Empty<LockedCellDto>();

    }
}
