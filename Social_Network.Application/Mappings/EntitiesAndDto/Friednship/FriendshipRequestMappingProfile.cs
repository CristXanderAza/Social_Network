using AutoMapper;
using Social_Network.Core.Application.DTOs.FriendshipRequest;
using Social_Network.Core.Domain.Entities.FriendshipRequests;

namespace Social_Network.Core.Application.Mappings.EntitiesAndDto.Friednship
{
    public class FriendshipRequestMappingProfile : Profile
    {
        public FriendshipRequestMappingProfile()
        {
            CreateMap<FriendshipRequestInsertDTO, FriendshipRequest>();
        }
    }
}
