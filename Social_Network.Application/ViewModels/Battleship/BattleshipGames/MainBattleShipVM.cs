namespace Social_Network.Core.Application.ViewModels.Battleship.BattleshipGames
{
    public class MainBattleShipVM
    {
        public IEnumerable<HistoryGameVM> HistoryGameVMs { get; set; }
        public IEnumerable<ActiveGameVM> ActiveGameVMs { get; set; }
        public BattleShipResumeVM BattleShipResumeVM { get; set; }
    }
}
