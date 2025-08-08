using Social_Network.Core.Application.DTOs.FriendshipRequest;
using Social_Network.Core.Application.DTOs.Friendships;
using Social_Network.Core.Domain.Base;

namespace Social_Network.Core.Application.Interfaces.Services.Friendships
{
    public interface IFriendshipService
    {
        Task<Result<Unit>> CreateRequest(FriendshipRequestInsertDTO request);
        Task<Result<Unit>> ApproveRequest(Guid requesId, string userId);
        Task<Result<Unit>> RejectRequest(Guid requesId, string userId);
        Task<Result<Unit>> DeleteRequest(Guid requesId, string userId);
        Task<IEnumerable<FriendshipRequestDTO>> GetPendingRequest(string userId);    
        Task<IEnumerable<FriendDTO>> GetFriends(string userId);
        Task<Result<Unit>> DeleteFriendship(Guid frienshipId ,string userId);
        Task<FriendDTO> GetFriendShip(Guid friendshipId, string userId);
        Task<IEnumerable<FriensOptionDTO>> GetPosibleFriendsFor(string userId);
        Task<string> GetNameOfOtherUserInRequest(Guid RequestId, string userId);
    }
}
