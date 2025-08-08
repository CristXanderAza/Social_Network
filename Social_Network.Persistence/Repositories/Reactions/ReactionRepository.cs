using Social_Network.Core.Application.Interfaces.Repositories.Reactions;
using Social_Network.Core.Domain.Entities.Reactions;
using Social_Network.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infraestructure.Persistence.Repositories.Reactions
{
    public class ReactionRepository : BaseRepository<Reaction, Guid>, IReactionRepository
    {
        public ReactionRepository(SocialNetworkContext context) : base(context)
        {
        }

        public async Task<Reaction?> GetReactionOfUserInPost(Guid postId, string userId)
            => _context.Reactions.FirstOrDefault(r => r.UserId == userId && r.PostId == postId);
    }
}
