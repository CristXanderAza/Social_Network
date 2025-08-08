using AutoMapper;
using Social_Network.Core.Application.DTOs.Posts;
using Social_Network.Core.Application.ViewModels.Home;
using Social_Network.Core.Application.ViewModels.Posts;

namespace Social_Network.Core.Application.Mappings.DtoAndViewModel
{
    public class GeneralMappingProfile : Profile
    {
        public GeneralMappingProfile()
        {
            CreateMap<PostWritteVM, PostInsertDTO>()
                 .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<HomeVM, PostInsertDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<PostDTO, PostReadVM>();
        }
    }
}
