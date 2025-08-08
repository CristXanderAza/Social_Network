using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Social_Network.Core.Application.DTOs.Users;
using Social_Network.Core.Application.Interfaces.Services.Users;
using Social_Network.Core.Application.ViewModels.Users;
using Social_Network.Core.Domain.Base;
using Social_Network.Infraestructure.Identity.Entities;
using Social_Network.Presentation.WebApp.Helpers;
using System.Security.Claims;

namespace Social_Network.WebApp.Controllers.Users
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<IdentUser> _userManager;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper, UserManager<IdentUser> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? errors = null, string? Message = null)
        {
            ViewBag.Errors = errors;    
            ViewBag.Message = Message;
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                return RedirectToAction("Index","Home");
            }
            var vm = new LoginVM();
            return View(vm);
        }

        [HttpPost]                              
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", vm);
            }
            var result = await _userService.Login(_mapper.Map<LoginRequestDTO>(vm));
            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", new { errors = result.Error });
        }

        public IActionResult Register()
        {
            var vm = new RegisterRequestVM();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            string origin = Request.Headers["origin"].ToString();
            var registerRequest = _mapper.Map<RegisterRequestDTO>(vm);
            var result = await _userService.RegisterAsync(registerRequest, origin);
            if (result.IsSuccess)
            {
                string photoPath = UploadFileHelper.SaveFile(vm.PhotoPath, result.Value, "Profile_Photo");
                if (!string.IsNullOrEmpty(photoPath))
                {
                    var res = await _userService.UpdateProfilePhoto(result.Value, photoPath);
                    if (!res.IsSuccess)
                    {
                        return RedirectToAction("Index", new { Eror = res.Error });
                        return View(vm);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
           return RedirectToAction("Index", new { errors = result.Error});
            return View(vm);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                ViewBag.ErrorMessage = "Invalid email confirmation link.";
                return View("Error");
            }
            var result = _userService.ConfirmEmailAsync(userId, token).Result;
            if (result.IsSuccess)
            {
                return RedirectToAction("Index", new { Message = "EmailSended" });
            }
            return RedirectToAction("Index", new { errors = result.Error });
            return View("Error");
        }

        public async Task<IActionResult> EditUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", new {erros = "No esta logeado para editar"});
            }
            var editVm = new EditUserVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
            };
            return View(editVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM vm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", new { erros = "No esta logeado para editar" });
            }
            var photoPath = UploadFileHelper.SaveFile(vm.Photo, userId, "Profile_Photo", true, user.PhotoPath);
            var updateDto = new UserUpdateDTO
            {
                Id = user.Id,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Password = vm.Password,
                PhoneNumber = vm.PhoneNumber,
                PhotoPath = photoPath,
            };
            var res = await _userService.EditUser(updateDto);
            return RedirectToAction("Index", new { errors = res.Error});
        }

        public async Task<IActionResult> Logout()
        {
            await _userService.SignOut();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ForgottPassword()
        {
            var vm = new ForgottPasswordVM();
            return View("ForgottPassword", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ForgottPassword(ForgottPasswordVM vm)
        {
            string origin = Request.Headers["origin"].ToString();
            var res = await _userService.SendResetPasswordEmail(vm.Email, origin);
            if(res.IsSuccess)
                return RedirectToAction("Index", new { Message = "Se le envio el correo de restauracion" });
            return RedirectToAction("Index", new { errors = res.Error });
        }

        public IActionResult ResetPassword(string token, string userId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            return View(new ResetPasswordVM
            {
                Token = token,
                UserId = userId
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _userService.ResetPasswordAsync(vm.UserId, vm.Token, vm.Password);

            if (!result.IsSuccess)
            {
                return RedirectToAction("Index", new { errors = result.Error});
            }

            return RedirectToAction("Index", new {Message = "Se el ha reiniciado la contraseña"});
        }

    }
}
