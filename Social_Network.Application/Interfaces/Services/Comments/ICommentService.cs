using Social_Network.Core.Application.DTOs.Comments;
using Social_Network.Core.Domain.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Services.Comments
{
    public interface ICommentService : IServiceBase<Comment, Guid, CommentDTO, CommentInsertDTO, CommentUpdateDTO>
    {

    }
}
