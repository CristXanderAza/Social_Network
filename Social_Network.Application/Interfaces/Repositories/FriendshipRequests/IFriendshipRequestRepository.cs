using Social_Network.Core.Domain.Entities.FriendshipRequests;

namespace Social_Network.Core.Application.Interfaces.Repositories.FriendshipRequests
{
    public interface IFriendshipRequestRepository : IRepositoryBase<FriendshipRequest, Guid>
    {
        Task<IEnumerable<FriendshipRequest>> GetPendingsRelatedToUser(string userId); 
    }
}
