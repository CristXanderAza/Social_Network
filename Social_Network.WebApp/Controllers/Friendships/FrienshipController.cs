using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.DTOs.Posts;
using Social_Network.Core.Application.Interfaces.Services.Friendships;
using Social_Network.Core.Application.Interfaces.Services.Posts;
using Social_Network.Core.Application.ViewModels.Common;
using Social_Network.Core.Application.ViewModels.Friendships;
using Social_Network.Core.Application.ViewModels.Posts;
using Social_Network.Core.Domain.Entities.Reactions;
using System.Security.Claims;

namespace Social_Network.WebApp.Controllers.Friendships
{
    [Authorize]
    public class FrienshipController : Controller
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public FrienshipController(IFriendshipService friendshipService, IPostService postService, IMapper mapper)
        {
            _friendshipService = friendshipService;
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string? error = null)
        {
            ViewBag.Errors = error;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var posts = await _postService.GetFriendsPosts(userId);
            var friends = await _friendshipService.GetFriends(userId);
            var postVms = posts.Select(p => _mapper.Map<PostReadVM>(p));
            var friendsVms = friends.Select(f => _mapper.Map<FriendReadVM>(f));
            return View(new FriendshipMainVM
            {
                Friends = friendsVms,
                Posts = postVms
            });
        }

        public async Task<IActionResult> GetProfile(string userId)
        {
            var profileDto = await _postService.GetProfileOfUser(userId);
            var vm = _mapper.Map<ProfileReadVM>(profileDto);
            return View(vm);
        }

        public async Task<IActionResult> DeleteFriendship(Guid friendShipId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = await _friendshipService.GetFriendShip(friendShipId, userId);
            var dangerAletVm = new GenericAlertVM
            {
                ResourceId = dto.FriendshipId,
                Message = "¿Estas seguro que deseas terminar esta amistad",
                Controller = "Frienship",
                ActionDestination = "DeleteFriendship"
            };
            return View("GenericAlert", dangerAletVm);
        }

        public async Task<IActionResult> DeleteFriendship(GenericAlertVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _friendshipService.DeleteFriendship(vm.ResourceId, userId);
            return RedirectToAction("Index", res.Error);
        }

        public async Task<IActionResult> GiveLikeToPost(Guid postId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _postService.ReactToPost(new ReactToPostDTO
            {
                PostId = postId,
                UserId = userId,
                ReactionId = (int)ReactionType.Like
            });
            return RedirectToAction("Index", res.Error);
        }

        public async Task<IActionResult> GiveDislikeToPost(Guid postId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _postService.ReactToPost(new ReactToPostDTO
            {
                PostId = postId,
                UserId = userId,
                ReactionId = (int)ReactionType.Dislike
            });
            return RedirectToAction("Index", res.Error);
        }
    }
}
