using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Interfaces.Repositories.FriendshipRequests;
using Social_Network.Core.Domain.Entities.FriendshipRequests;
using Social_Network.Infraestructure.Persistence.Context;

namespace Social_Network.Infraestructure.Persistence.Repositories.FriendshipRequests
{
    public class FriendshipRequestRepository : BaseRepository<FriendshipRequest, Guid>, IFriendshipRequestRepository
    {
        public FriendshipRequestRepository(SocialNetworkContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FriendshipRequest>> GetPendingsRelatedToUser(string userId)
            => await _context.FriendshipRequests
                       .Where(fr => (fr.FromUserId == userId || fr.ToUserId == userId) && fr.Status == FriendshipRequestStatus.Pending)
                       .ToListAsync();
    }
}
