using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Friendships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories.Friendships
{
    public interface IFriendshipRepository : IRepositoryBase<Friendship, Guid>
    {
        Task<IEnumerable<Friendship>> GetFriendsOf(string userId);
        Task<IEnumerable<string>> GetFriendsId(string userId);
        Task<int> GetFriendsInComonWith(string userId, string otherUserId);
        Task<Dictionary<string, int>> GetFriendsInComonWithMany(string userId, IEnumerable<string> otherUserIds);
        Task<Dictionary<string, int>> GetFriendsInComonWithMany(IEnumerable<string> friendsIds, IEnumerable<string> otherUserIds);
    }
}
