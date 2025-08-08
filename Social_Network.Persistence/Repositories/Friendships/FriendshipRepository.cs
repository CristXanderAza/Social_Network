using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Interfaces.Repositories.Friendships;
using Social_Network.Core.Domain.Entities.Friendships;
using Social_Network.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infraestructure.Persistence.Repositories.Friendships
{
    public class FriendshipRepository : BaseRepository<Friendship, Guid>, IFriendshipRepository
    {
        public FriendshipRepository(SocialNetworkContext context) : base(context)
        {
        }

        public async Task<IEnumerable<string>> GetFriendsId(string userId)
            => await _context.Friendships
                            .Where(f => f.FromUserId == userId || f.ToUserId == userId)
                            .Select(f => f.FromUserId == userId ? f.ToUserId : f.FromUserId)
                            .ToListAsync();

        public async Task<int> GetFriendsInComonWith(string userA, string userB)
        {
            var amigosDeA = _context.Friendships
                            .Where(f => f.FromUserId == userA || f.ToUserId == userA)
                            .Select(f => f.FromUserId == userA ? f.ToUserId : f.FromUserId);

            var amigosDeB = _context.Friendships
                .Where(f => f.FromUserId == userB || f.ToUserId == userB)
                .Select(f => f.FromUserId == userB ? f.ToUserId : f.FromUserId);

            var amigosEnComun = amigosDeA.Intersect(amigosDeB);

            return await amigosEnComun.CountAsync();
        }

        public Task<Dictionary<string, int>> GetFriendsInComonWithMany(string userId, IEnumerable<string> otherUserIds)
        {
            var myFriends = _context.Friendships
                            .Where(f => f.FromUserId == userId || f.ToUserId == userId)
                            .Select(f => f.FromUserId == userId ? f.ToUserId : f.FromUserId)
                           ;

            var fromOtherFriends = _context.Friendships
                                    .Where(f => myFriends.Contains(f.FromUserId))
                                    .GroupBy(f => f.ToUserId)
                                    .Select(g => new { key = g.Key, Count = g.Count() });
            var toOtherFriends = _context.Friendships
                        .Where(f => myFriends.Contains(f.ToUserId))
                        .GroupBy(f => f.FromUserId)
                        .Select(g => new { key = g.Key, Count = g.Count() });

            return fromOtherFriends.Union(toOtherFriends)
                .Where(x => otherUserIds.Contains(x.key))
                .ToDictionaryAsync(d => d.key, d => d.Count);
        }

        public Task<Dictionary<string, int>> GetFriendsInComonWithMany(IEnumerable<string> friendsIds, IEnumerable<string> otherUserIds)
        {
            var fromOtherFriends = _context.Friendships
                        .Where(f => friendsIds.Contains(f.FromUserId))
                        .GroupBy(f => f.ToUserId)
                        .Select(g => new { key = g.Key, Count = g.Count() });
            var toOtherFriends = _context.Friendships
                        .Where(f => friendsIds.Contains(f.ToUserId))
                        .GroupBy(f => f.FromUserId)
                        .Select(g => new { key = g.Key, Count = g.Count() });

            return fromOtherFriends.Union(toOtherFriends)
                .Where(x => otherUserIds.Contains(x.key))
                .ToDictionaryAsync(d => d.key, d => d.Count);
        }

        public async Task<IEnumerable<Friendship>> GetFriendsOf(string userId)
        {
            return await _context.Friendships
                .Where(f => f.FromUserId == userId || f.ToUserId == userId)
                .ToListAsync();
        }
    }
}
