using AutoMapper;
using Social_Network.Core.Application.DTOs.Posts;
using Social_Network.Core.Application.DTOs.Users;
using Social_Network.Core.Application.Interfaces.Repositories.Posts;
using Social_Network.Core.Application.Interfaces.Repositories.Reactions;
using Social_Network.Core.Application.Interfaces.Services.Posts;
using Social_Network.Core.Application.Interfaces.Services.Users;
using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Posts;
using Social_Network.Core.Domain.Entities.Reactions;

namespace Social_Network.Core.Application.Services.Posts
{
    public class PostService : ServiceBase<Post, Guid, PostDTO, PostInsertDTO, PostUpdateDTO>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly IUserService _userService;

        public PostService(IPostRepository repositoryBase, IMapper mapper, IUserService userService, IReactionRepository reactionRepository) : base(repositoryBase, mapper)
        {
            _postRepository = repositoryBase;
            _userService = userService;
            _reactionRepository = reactionRepository;
            configure();            
        }

        public async Task<IEnumerable<PostDTO>> GetFriendsPosts(string userId)
        {
            var posts = await _postRepository.GetPostsOfFriendsOfUser(userId);
            var dtos = posts.Select(p => _mapper.Map<PostDTO>(p));
            var headers = await _userService.GetHeadersOfUsers(dtos.Select(c => c.UserId).ToList());
            return dtos.Select(dto =>
            {
                var header = headers[dto.UserId];
                dto.UserProfilePhotoPath = header.ProfilePhotoPath;
                dto.AuthorUserName = header.UserName;
                return dto;
            });
        }

        public async Task<ProfileDTO> GetProfileOfUser(string userId)
        {
            var posts = await _postRepository.GetPostsOfUser(userId);
            var dtos = posts.Select(p => _mapper.Map<PostDTO>(p));
            var header = await _userService.GetHeaderOfUsers(userId);
            dtos = dtos.Select(dto =>
            {
                dto.UserProfilePhotoPath = header.ProfilePhotoPath;
                dto.AuthorUserName = header.UserName;
                return dto;
            });
            return new ProfileDTO
            {
                PostDTOs = dtos,
                ProfileImgURL = header.ProfilePhotoPath,
                UserName = header.UserName,
                UserId = userId
            };
        }

        public async Task<IEnumerable<PostDTO>> GetPostsOfUser(string userId)
        {
            var posts = await _postRepository.GetPostsOfUser(userId);
            var dtos = posts.Select(p => _mapper.Map<PostDTO>(p));
            var header = await _userService.GetHeaderOfUsers(userId);
            return dtos.Select(dto =>
            {
                dto.UserProfilePhotoPath = header.ProfilePhotoPath;
                dto.AuthorUserName = header.UserName;
                return dto;
            });
        }

        public async Task<PostDTO> GetPostWithCommentsAndReactions(Guid postId)
        {
            var post = await _postRepository.GetPostWithCommentsAndReactions(postId);
            var postDto = _mapper.Map<PostDTO>(post);
            var usersIds = post.Comments.Select(c => c.UserId).ToList();
            usersIds.Add(post.UserId);
            var headers = await _userService.GetHeadersOfUsers(usersIds.Distinct());
            postDto.AuthorUserName = headers[postDto.UserId].UserName;
            postDto.UserProfilePhotoPath = headers[postDto.UserId].ProfilePhotoPath;
            postDto.Comments = postDto.Comments.Select(c =>
            {
                var header = headers[c.UserId];
                c.UserPhotoPath = header.ProfilePhotoPath;
                c.UserName = header.UserName;
                return c;
            });
            return postDto;

        }

        public async Task<Result<Unit>> ReactToPost(ReactToPostDTO reacDTO)
        {
            var reaction = await _reactionRepository.GetReactionOfUserInPost(reacDTO.PostId, reacDTO.UserId);
            if (reaction == null)
            {
                reaction = new Reaction
                {
                    PostId = reacDTO.PostId,
                    UserId = reacDTO.UserId,
                    Type = (ReactionType)reacDTO.ReactionId
                };
               var resCre =  await _reactionRepository.AddAsync(reaction);
                if(resCre.IsSuccess)
                {
                    return Unit.Value;
                }
                else
                {
                    return Result<Unit>.Fail(resCre.Error);
                }
            }
            reaction.Type = (ReactionType) reacDTO.ReactionId;
            var resUp = await _reactionRepository.UpdateAsync(reaction);
            if (resUp.IsSuccess)
            {
                return Unit.Value;
            }
            else
            {
                return Result<Unit>.Fail(resUp.Error);
            }
            
        }

        private void configure()
        {
            ConfigureUpdatePreCondition((dto, ent)
                => ent.UserId == dto.UserId ? dto : Result<PostUpdateDTO>.Fail("You cannot Update a Post that is not yours"));
            ConfigureDeletePreCondition((id, UserId, ent)
                => ent.UserId == UserId ? Unit.Value : Result<Unit>.Fail("You cannot Delete a Post that is not yours"));
            ConfigureGetAllPostMapping(async dtos
                =>
            {
                var headers = await _userService.GetHeadersOfUsers(dtos.Select(c => c.UserId).ToList());
                return dtos.Select(dto =>
                {
                    var header = headers[dto.UserId];
                    dto.UserProfilePhotoPath = header.ProfilePhotoPath;
                    dto.AuthorUserName = header.UserName;
                    return dto;
                });
            });
            ConfigureGetByIdPostMapping(async dto
                =>
            {
                var header = await _userService.GetHeaderOfUsers(dto.UserId);
                dto.AuthorUserName = header.UserName.ToLower();
                dto.UserProfilePhotoPath = header.ProfilePhotoPath;
                return dto;
            });
        }
    }
}
