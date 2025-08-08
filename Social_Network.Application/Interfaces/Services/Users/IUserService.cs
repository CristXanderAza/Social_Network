using Social_Network.Core.Application.DTOs.Users;
using Social_Network.Core.Domain.Base;

namespace Social_Network.Core.Application.Interfaces.Services.Users
{
    public interface IUserService
    {
        Task<Result<Unit>> ConfirmEmailAsync(string userId, string token);
        Task<Result<Unit>> DeleteUserAsyn(string id);
        Task<Result<Unit>> EditUser(UserUpdateDTO dto);
        Task<Result<Unit>> UpdateProfilePhoto(string userId, string photoPath);
        Task<Result<LoginResponseDTO>> Login(LoginRequestDTO loginRequest);
        Task<Result<string>> RegisterAsync(RegisterRequestDTO registerRequest, string origin);
        Task<Result<Unit>> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<Result<Unit>> SendResetPasswordEmail(string email, string origin);
        Task SignOut();
        Task<Dictionary<string, UserHeaderDTO>> GetHeadersOfUsers(IEnumerable<string> ids);
        Task<UserHeaderDTO> GetHeaderOfUsers(string id);
        Task<IEnumerable<UserHeaderDTO>> GetHeadersOfNonFriends(IEnumerable<string> friendsIds);
    }
}