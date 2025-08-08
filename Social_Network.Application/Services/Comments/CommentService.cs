using AutoMapper;
using Social_Network.Core.Application.DTOs.Comments;
using Social_Network.Core.Application.Interfaces.Repositories.Comments;
using Social_Network.Core.Application.Interfaces.Repositories.Posts;
using Social_Network.Core.Application.Interfaces.Services.Comments;
using Social_Network.Core.Application.Interfaces.Services.Users;
using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Comments;

namespace Social_Network.Core.Application.Services.Comments
{
    public class CommentService : ServiceBase<Comment, Guid, CommentDTO, CommentInsertDTO, CommentUpdateDTO>, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserService _userService;
        private readonly IPostRepository _postRepository;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IPostRepository postRepository, IUserService userService) : base(commentRepository, mapper)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userService = userService;
            Configure();

        }

        private void Configure()
        {
            ConfigureInsertPreCondition(async insDto
                =>
            {
                bool existPost = await _postRepository.Any(p => p.Id == insDto.PostId);
                if (!existPost)
                    return Result<CommentInsertDTO>.Fail("Post for comment not found");
                if (insDto.ParentCommentId != null)
                {
                    bool existParentComment = await _commentRepository.Any(c => c.Id == insDto.ParentCommentId);
                    if (!existParentComment)
                        return Result<CommentInsertDTO>.Fail("Comment to reply not found");
                }
                return insDto;
            });
            ConfigureUpdatePreCondition((dto, ent)
                => ent.UserId == dto.UserId ? dto : Result<CommentUpdateDTO>.Fail("You cannot Update a Comment that is not yours"));
            ConfigureDeletePreCondition((id, UserId, ent)
                =>
            {
                
                return ent.UserId == UserId ? Unit.Value : Result<Unit>.Fail("You cannot Delete a Comment that is not yours");
            });

            ConfigureGetAllPostMapping(async dtos
             =>
            {
                var headers = await _userService.GetHeadersOfUsers(dtos.Select(c => c.UserId).ToList());
                return dtos.Select(dto =>
                {
                    var header = headers[dto.UserId];
                    dto.UserPhotoPath = header.ProfilePhotoPath;
                    dto.UserName = header.UserName;
                    return dto;
                });
            });
            ConfigureGetByIdPostMapping(async dto
             =>
            {
                var header = await _userService.GetHeaderOfUsers(dto.UserId);
                dto.UserName = header.UserName.ToLower();
                dto.UserPhotoPath = header.ProfilePhotoPath;
                return dto;
            });
        }
    }
}
