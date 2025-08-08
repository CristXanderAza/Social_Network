using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.DTOs.Comments;
using Social_Network.Core.Application.Interfaces.Services.Comments;
using Social_Network.Core.Application.Interfaces.Services.Posts;
using Social_Network.Core.Application.ViewModels.Comments;
using Social_Network.Core.Application.ViewModels.Common;
using System.Security.Claims;

namespace Social_Network.WebApp.Controllers.Comments
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;

        public CommentController(IMapper mapper, ICommentService commentService, IPostService postService)
        {
            _mapper = mapper;
            _commentService = commentService;
            _postService = postService;
        }

        public async Task<IActionResult> ShowCommentsOf(Guid id, string? erros = null)
        {
            ViewBag.Errors = erros;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var completePost = await _postService.GetPostWithCommentsAndReactions(id);
            var comments = completePost.Comments;
            var vm = new CommentListVM
            {
                PostId = completePost.Id,
                UserName = completePost.AuthorUserName,
                ActualUserId = userId,
                UserProfilePhotoUrl = completePost.UserProfilePhotoPath,
                PostText = completePost.Content,
                VideoUrl = completePost.VideoUrl,
                ImgUrl = completePost.ImageUrl,
                ReadVMs = comments.Select(c => _mapper.Map<CommentReadVM>(c))
            };
            return View(vm);
        }

        public async Task<IActionResult> NewCommentFor(Guid id)
        {
            var vm = new CommentWritteVM
            {
                PostId = id,
                IsCreating = true,

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> NewCommentFor(CommentWritteVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (vm.IsCreating)
            {
                
                var dto = new CommentInsertDTO
                {
                    PostId = vm.PostId,
                    Content = vm.Content,
                    ParentCommentId = vm.ParentCommentId,
                    UserId = userId
                };
                var res = await _commentService.CreateAsync(dto);
                return RedirectToAction("ShowCommentsOf", new { errors = res.Error, id = vm.PostId });

            }
            else
            {
                var updateDto = new CommentUpdateDTO
                {
                    UserId = userId,
                    Content = vm.Content,
                    Id = vm.CommentId.Value,
                };
                var res = await _commentService.UpdateAsync(updateDto);
                return RedirectToAction("ShowCommentsOf", new { errors = res.Error, id = vm.PostId });
            }

        }

        public async Task<IActionResult> NewRepplyFor(Guid commentId, Guid postId)
        {
            var vm = new CommentWritteVM
            {
                PostId = postId,
                ParentCommentId = commentId,
                IsCreating = true,

            };
            return View("NewCommentFor", vm);
        }

        public async Task<IActionResult> EditComment(Guid commentId, Guid postId)
        {
            var commentDto = await _commentService.GetById(commentId);
            if (commentDto.IsFailure)
            {
                return RedirectToAction("ShowCommentsOf", new { errors = commentDto.Error, id = postId });
            }
            var vm = new CommentWritteVM
            {
                PostId = postId,
                CommentId = commentId,
                IsCreating = false,
                Content = commentDto.Value.Content

            };
            return View("NewCommentFor", vm);
        }

        public async Task<IActionResult> DeleteComment(Guid commentId, Guid postId)
        {
            var dangerAletVm = new GenericAlertVM
            {
                ResourceId = commentId,
                AnotherResoruceId = postId,
                Message = "¿Estas seguro que deseas borrar este comentario",
                Controller = "Comment",
                ActionDestination = "DeleteComment"
            };
            return View("GenericAlert", dangerAletVm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(GenericAlertVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _commentService.DeleteAsync(vm.ResourceId, userId);
            return RedirectToAction("ShowCommentsOf", new { errors = res.Error, id = vm.AnotherResoruceId });
        }

    }
}
