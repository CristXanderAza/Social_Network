using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories.Comments
{
    public interface ICommentRepository : IRepositoryBase<Comment, Guid>
    {
        Task<Result<Unit>> DeboundCommentsFromParent(Guid? parentId);
    }
}
