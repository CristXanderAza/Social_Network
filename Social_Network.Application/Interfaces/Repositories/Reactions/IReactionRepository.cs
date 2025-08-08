using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Reactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories.Reactions
{
    public interface IReactionRepository : IRepositoryBase<Reaction, Guid>
    {
        Task<Reaction?> GetReactionOfUserInPost(Guid postId,string  userId);
    }
}
