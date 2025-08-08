using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.DTOs.Posts;
using Social_Network.Core.Application.Helpers.Enums;
using Social_Network.Core.Application.Interfaces.Services.Posts;
using Social_Network.Core.Application.ViewModels.Common;
using Social_Network.Core.Application.ViewModels.Home;
using Social_Network.Core.Application.ViewModels.Posts;
using Social_Network.Core.Domain.Entities.Posts;
using Social_Network.Core.Domain.Entities.Reactions;
using Social_Network.Presentation.WebApp.Helpers;
using System.Security.Claims;

namespace Social_Network.WebApp.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostService _postService;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger, IPostService postService, IMapper mapper)
    {
        _logger = logger;
        _postService = postService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string? errors = null)
    {
        ViewBag.Errors = errors;
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var postDto = await _postService.GetPostsOfUser(userId);
        var vms = postDto.Select(p => _mapper.Map<PostReadVM>(p)).ToList();
        var HomeVm = new HomeVM
        {
            ReadVMs = vms,
            UserId = userId,
            Types = EnumHelper.GetEnumsAsIdent<PostType>()
        };
        return View(HomeVm);
    }
    /*
    public async Task<IActionResult> NewPost()
    {
        var typePost = EnumHelper.GetEnumsAsIdent<PostType>();
        var vm = new PostWritteVM
        {
            Types = typePost,

        };
        return View(vm);
    }
    */
    public async Task<IActionResult> NewPost(HomeVM writteVM)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        writteVM.UserId = userId;
        if (!ModelState.IsValid)
        {
            writteVM.Types = EnumHelper.GetEnumsAsIdent<PostType>();
            writteVM.UserId = userId;
            var postDto = await _postService.GetPostsOfUser(userId);
            var vms = postDto.Select(p => _mapper.Map<PostReadVM>(p)).ToList();
            writteVM.ReadVMs = vms;
            return View("Index", writteVM);
        }
        PostInsertDTO insertDTO = null;
        //validaciones:
        if (writteVM.PostTypeId == (int)PostType.WithVideo && string.IsNullOrEmpty(writteVM.VideoUrl))
        {
            writteVM.Types = EnumHelper.GetEnumsAsIdent<PostType>();
            ViewBag.Error = "No ha anexado ningun video";
            return View("NewPost", writteVM);
        }
        if(writteVM.PostTypeId == (int)PostType.WithImage)
        {
            if(writteVM.Image == null)
            {
                writteVM.Types = EnumHelper.GetEnumsAsIdent<PostType>();
                ViewBag.Error = "No ha anexado ninguna imagen";
                return View("NewPost", writteVM);
            }
            var imgUrl = UploadFileHelper.SaveFile(writteVM.Image, userId, "PostImages");
            insertDTO = _mapper.Map<PostInsertDTO>(writteVM);
            insertDTO.ImageUrl = imgUrl;
            
        }
        else
        {
            insertDTO = _mapper.Map<PostInsertDTO>(writteVM);
        }
        var res = await _postService.CreateAsync(insertDTO);
        if (!res.IsSuccess)
        {
            return RedirectToAction("Index", res.Error);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> EditPost(Guid postId)
    {
        var resDto = await _postService.GetById(postId);
        if (!resDto.IsSuccess)
        {
            return RedirectToAction("Index", resDto.Error);
        }
        var post = resDto.Value;
        var vm = new PostUpdateVM
        {
            PostId = post.Id,
            Content = post.Content,
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> EditPost(PostUpdateVM postUpdateVM)
    {
        if (!ModelState.IsValid)
        {
            return View(postUpdateVM);
        }
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var dto = new PostUpdateDTO
        {
            Content = postUpdateVM.Content,
            Id = postUpdateVM.PostId,
            UserId = userId
        };
        var res = await _postService.UpdateAsync(dto);
        if (!res.IsSuccess)
        {
            return RedirectToAction("Index", res.Error);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeletePost(Guid PostId)
    {
       // var result = await _postService.GetById(PostId);
     /*   if (result.IsFailure)
        {
            return RedirectToAction("Index", "Elmento a eliminar no existe");
        }*/
        var deleteVm = new GenericAlertVM
        {
            Message = "Estas seguro que quieres borrar esta publicacion?",
            ResourceId = PostId,
            Controller = "Home",
            ActionSource = "Index",
            ActionDestination = "DeletePost"
        };
        return View("GenericAlert",deleteVm);

    }

    [HttpPost]
    public async Task<IActionResult> DeletePost(GenericAlertVM alertVM)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var res = await _postService.DeleteAsync(alertVM.ResourceId, userId);
        if (!res.IsSuccess)
        {
            return RedirectToAction("Index", res.Error);
        }
        return RedirectToAction("Index");
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
