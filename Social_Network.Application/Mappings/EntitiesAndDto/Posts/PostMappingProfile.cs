using AutoMapper;
using Social_Network.Core.Application.DTOs.Posts;
using Social_Network.Core.Domain.Entities.Comments;
using Social_Network.Core.Domain.Entities.Posts;
using Social_Network.Core.Domain.Entities.Reactions;

namespace Social_Network.Core.Application.Mappings.EntitiesAndDto.Posts
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile() 
        {
            CreateMap<Post, PostDTO>()
                 .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.UpdatedAt))
                 .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Reactions != null? src.Reactions.Where(r => r.Type == ReactionType.Like).Count() : 0))
                 .ForMember(dest => dest.Dislikes, opt => opt.MapFrom(src => src.Reactions != null ? src.Reactions.Where(r => r.Type == ReactionType.Dislike).Count() : 0))
                 .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments != null? src.Comments : Enumerable.Empty<Comment>()));

            CreateMap<PostInsertDTO, Post>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.PostTypeId == (int)PostType.WithImage? src.ImageUrl : null))
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.PostTypeId == (int)PostType.WithVideo ? src.VideoUrl : null));

            CreateMap<PostUpdateDTO, Post>();
        
        }
    }
}
