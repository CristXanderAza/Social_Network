using AutoMapper;
using Social_Network.Core.Application.DTOs.Battleship.BattleshipGames;
using Social_Network.Core.Application.ViewModels.Battleship.BattleshipGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Mappings.DtoAndViewModel
{
    public class BattleshipGameVMMappingProfile : Profile
    {
        public BattleshipGameVMMappingProfile()
        {
            CreateMap<GameCurrentInfoDTO, CurrentGameInfoVM>();
            CreateMap<BattleShipGamesResumeDTO, BattleShipResumeVM>();
            CreateMap<BattleShipGameDTO, ActiveGameVM>();
            CreateMap<GameHistoryDTO, HistoryGameVM>();
        }
    }
}
