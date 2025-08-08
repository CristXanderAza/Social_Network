using AutoMapper;
using Social_Network.Core.Application.DTOs.FriendshipRequest;
using Social_Network.Core.Application.DTOs.Friendships;
using Social_Network.Core.Application.DTOs.Users;
using Social_Network.Core.Application.ViewModels.FriendshipRequest;
using Social_Network.Core.Application.ViewModels.Friendships;

namespace Social_Network.Core.Application.Mappings.DtoAndViewModel
{
    public class FriendshipVMMappingProfile : Profile
    {
        public FriendshipVMMappingProfile()
        {
            CreateMap<FriendDTO, FriendReadVM>();
            CreateMap<FriendshipRequestDTO, FriendshipRequestVM>();
            CreateMap<ProfileDTO, ProfileReadVM>();
            CreateMap<FriensOptionDTO, FriendOptionVM>();
        }
    }
}
