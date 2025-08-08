using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.DTOs.FriendshipRequest;
using Social_Network.Core.Application.Interfaces.Services.Friendships;
using Social_Network.Core.Application.ViewModels.Common;
using Social_Network.Core.Application.ViewModels.FriendshipRequest;
using System.Security.Claims;

namespace Social_Network.WebApp.Controllers.FriendshipRequest
{
    [Authorize]
    public class FriendshipRequestController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFriendshipService _friendshipService;

        public FriendshipRequestController(IMapper mapper, IFriendshipService friendshipService)
        {
            _mapper = mapper;
            _friendshipService = friendshipService;
        }

        public async Task<IActionResult> Index(string? errors = null)
        {
            ViewBag.Errors = errors;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pending = await _friendshipService.GetPendingRequest(userId);
            var vm = new FriendshipRequestMainVM
            {
                RequestFrom = pending.Where(frr => frr.IsFromUser).Select(frr => _mapper.Map<FriendshipRequestVM>(frr)).ToList(),
                RequestTo = pending.Where(frr => !frr.IsFromUser).Select(frr => _mapper.Map<FriendshipRequestVM>(frr)).ToList(),
            };
            return View(vm);
        }

        public async Task<IActionResult> Save()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var posibleFriends = await _friendshipService.GetPosibleFriendsFor(userId);
            var vms = posibleFriends.Select(f => _mapper.Map<FriendOptionVM>(f)).ToList();
            var writteVm = new FriendRequestWritteVM { Options = vms };
            return View(writteVm);
        }

        [HttpPost]
        public async Task<IActionResult> Save(FriendRequestWritteVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var insertDto = new FriendshipRequestInsertDTO
            {
                FromUserId = userId,
                ToUserId = vm.SelectedOption
            };
            var res = await _friendshipService.CreateRequest(insertDto);
            return RedirectToAction("Index", new {errors = res.Error});
        }

        public async Task<IActionResult> RejectRequest(Guid requestId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = await _friendshipService.GetNameOfOtherUserInRequest(requestId, userId);
            var vm = new GenericAlertVM
            {
                Controller = "FriendshipRequest",
                ActionSource = "Index",
                ActionDestination = "RejectRequest",
                alertType = "danger",
                ResourceId = requestId,
                Message = "Esta seguro de que quiere rechazar la solicitud de amistad con " + name
            };
            return View("GenericAlert", vm);
        }

        public async Task<IActionResult> ApproveRequest(Guid requestId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = await _friendshipService.GetNameOfOtherUserInRequest(requestId, userId);
            var vm = new GenericAlertVM
            {
                Controller = "FriendshipRequest",
                ActionSource = "Index",
                ActionDestination = "ApproveRequest",
                alertType = "primary",
                ResourceId = requestId,
                Message = "Esta seguro de que quiere aceptar la solicitud de amistad con " + name
            };
            return View("GenericAlert", vm);
        }

        public async Task<IActionResult> DeleteRequest(Guid requestId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = await _friendshipService.GetNameOfOtherUserInRequest(requestId, userId);
            var vm = new GenericAlertVM
            {
                Controller = "FriendshipRequest",
                ActionSource = "Index",
                ActionDestination = "DeleteRequest",
                alertType = "danger",
                ResourceId = requestId,
                Message = "Esta seguro de que quiere borrar la solicitud de amistad con " + name
            };
            return View("GenericAlert", vm);
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(GenericAlertVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _friendshipService.RejectRequest(vm.ResourceId, userId);
            return RedirectToAction("Index", new { errors = res.Error });
        }
        [HttpPost]
        public async Task<IActionResult> ApproveRequest(GenericAlertVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _friendshipService.ApproveRequest(vm.ResourceId, userId);
            return RedirectToAction("Index", new { errors = res.Error });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRequest(GenericAlertVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _friendshipService.DeleteRequest(vm.ResourceId, userId);
            return RedirectToAction("Index", new { errors = res.Error });
        }



    }
}
