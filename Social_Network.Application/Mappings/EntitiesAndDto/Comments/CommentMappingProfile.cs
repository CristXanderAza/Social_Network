using AutoMapper;
using Social_Network.Core.Application.DTOs.Comments;
using Social_Network.Core.Domain.Entities.Comments;

namespace Social_Network.Core.Application.Mappings.EntitiesAndDto.Comments
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile() 
        {
            CreateMap<Comment, CommentDTO>();
            CreateMap <CommentInsertDTO, Comment>();
            CreateMap<CommentUpdateDTO, Comment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); ;
        }
    }
}
