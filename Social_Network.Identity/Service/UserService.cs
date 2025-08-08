using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.DTOs.Email;
using Social_Network.Core.Application.DTOs.Users;
using Social_Network.Core.Application.Interfaces.Services.Email;
using Social_Network.Core.Application.Interfaces.Services.Users;
using Social_Network.Core.Domain.Base;
using Social_Network.Infraestructure.Identity.Entities;
using System.Text;

namespace Social_Network.Infraestructure.Identity.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentUser> _userManager;
        private readonly SignInManager<IdentUser> _signInManager;
        private readonly IEmailService _emailService;

        public UserService(UserManager<IdentUser> userManager, SignInManager<IdentUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<Result<LoginResponseDTO>> Login(LoginRequestDTO loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                return Result<LoginResponseDTO>.Fail("Invalid email or password.");
            }
            if (!user.EmailConfirmed)
            {
                return Result<LoginResponseDTO>.Fail("Email not confirmed. Please check your inbox.");
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);
            if (!result.Succeeded)
            {
                return Result<LoginResponseDTO>.Fail("Invalid email or password.");
            }
            return new LoginResponseDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task SignOut()
            => await _signInManager.SignOutAsync();

        public async Task<Result<string>> RegisterAsync(RegisterRequestDTO registerRequest, string origin)
        {
            var userWithEmail = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (userWithEmail != null)
            {
                return Result<string>.Fail("Email already exists.");
            }
            var userWithUserName = await _userManager.FindByNameAsync(registerRequest.UserName);
            if (userWithUserName != null)
            {
                return Result<string>.Fail("Username already exists.");
            }
            var user = new IdentUser
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PhoneNumber = registerRequest.PhoneNumber,
                PhotoPath = "Placeholder",
                PhoneNumberConfirmed = true,
                EmailConfirmed = false
            };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                return Result<string>.Fail("Registration failed. Please try again.");
            }
            await _userManager.AddToRoleAsync(user, "BasicUser");
          
            var emailVerificationUrl = await GetEmailVerificationUrl(user, origin);
            await _emailService.SendAsync(new EmailRequestDTO
            {
                To = user.Email,
                Subject = "Confirm your email",
                BodyHtml = $"Confirm your email: <a>{emailVerificationUrl}</a>"
            });
            return user.Id;

        }

        public async Task<Result<Unit>> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("Email confirmation failed.");
            }
            return Unit.Value;
        }

        public async Task<Result<Unit>> EditUser(UserUpdateDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.PhotoPath = string.IsNullOrEmpty(dto.PhotoPath) ? user.PhotoPath : dto.PhotoPath;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("User update failed.");
            }
            // If password is provided, change it

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, resetPasswordToken, dto.Password);
                if (!passwordResult.Succeeded)
                {
                    return Result<Unit>.Fail("Password change failed.");
                }
            }
            return Unit.Value;
        }

        public async Task<Result<Unit>> UpdateProfilePhoto(string userId, string photoPath)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            user.PhotoPath = photoPath;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("Profile photo update failed.");
            }
            return Unit.Value;
        }

        public async Task<Result<Unit>> DeleteUserAsyn(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("User deletion failed.");
            }

            else
            {
                return Result<Unit>.Fail("Domain user deletion failed.");
            }
            return Unit.Value;
        }

        public async Task<Result<Unit>> SendResetPasswordEmail(string email, string origin)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "User/ResetPassword";
            var completeUrl = new Uri($"{origin}/{route}");
            var resetPasswordUrl = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id.ToString());
            resetPasswordUrl = QueryHelpers.AddQueryString(resetPasswordUrl, "token", token);
            await _emailService.SendAsync(new EmailRequestDTO
            {
                To = user.Email,
                Subject = "Reset your password",
                BodyHtml = $"Reset your password: <a>{resetPasswordUrl}</a>"
            });
            return Unit.Value;
        }

        public async Task<Result<Unit>> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("Password reset failed.");
            }
            return Unit.Value;
        }

        public async Task<Dictionary<string, UserHeaderDTO>> GetHeadersOfUsers(IEnumerable<string> ids)
            => await _userManager.Users.Where(u => ids.Contains(u.Id))
                     .ToDictionaryAsync(u => u.Id, U => new UserHeaderDTO
                     {
                         UserName = U.UserName,
                         UserId = U.Id,
                         ProfilePhotoPath = U.PhotoPath,
                         FirstName = U.FirstName,
                         LastName = U.LastName,
                     });


        public async Task<UserHeaderDTO> GetHeaderOfUsers(string id)
            => await _userManager.Users.Where(u => u.Id == id)
                     .Select(U => new UserHeaderDTO
                     {
                         UserName = U.UserName,
                         UserId = U.Id,
                         ProfilePhotoPath = U.PhotoPath,
                         FirstName = U.FirstName,
                         LastName = U.LastName,
                     }).FirstOrDefaultAsync();

        private async Task<string> GetEmailVerificationUrl(IdentUser user, string origin)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "User/ConfirmEmail";
            var completeUrl = new Uri($"{origin}/{route}");
            var verificationUrl = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id.ToString());
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "token", token);
            return verificationUrl;
        }

        public async Task<IEnumerable<UserHeaderDTO>> GetHeadersOfNonFriends(IEnumerable<string> friendsIds)
            => await _userManager.Users.Where(u => !friendsIds.Contains(u.Id))
                     .Select(U => new UserHeaderDTO
                     {
                         UserName = U.UserName,
                         UserId = U.Id,
                         ProfilePhotoPath = U.PhotoPath,
                         FirstName = U.FirstName,
                         LastName = U.LastName,
                     }).ToListAsync();
    }
}
