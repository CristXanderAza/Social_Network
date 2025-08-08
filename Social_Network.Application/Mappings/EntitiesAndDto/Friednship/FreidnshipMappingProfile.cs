using AutoMapper;
using Social_Network.Core.Application.DTOs.FriendshipRequest;
using Social_Network.Core.Domain.Entities.FriendshipRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Mappings.EntitiesAndDto.Friednship
{
    public class FreidnshipMappingProfile : Profile
    {
        public FreidnshipMappingProfile() 
        {
            CreateMap<FriendshipRequestInsertDTO, FriendshipRequest>();
        }
    }
}
