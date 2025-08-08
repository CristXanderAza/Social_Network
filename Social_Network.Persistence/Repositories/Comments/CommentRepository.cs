using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Interfaces.Repositories.Comments;
using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.Comments;
using Social_Network.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infraestructure.Persistence.Repositories.Comments
{
    public class CommentRepository : BaseRepository<Comment, Guid>, ICommentRepository
    {
        public CommentRepository(SocialNetworkContext context) : base(context)
        {
        }

        public async Task<Result<Unit>> DeboundCommentsFromParent(Guid? parentId)
        {
            if(parentId == null)
                return Unit.Value;
            try
            {
                var parentExists = await _context.Comments.AnyAsync(c => c.Id == parentId);
                if (!parentExists)
                {
                    return Result<Unit>.Fail($"No se encontró el comentario padre con ID {parentId}");
                }

                int affectedRows = await _context.Comments
                    .Where(c => c.ParentCommentId == parentId)
                    .ExecuteUpdateAsync(c => c
                        .SetProperty(c => c.ParentCommentId, (Guid?)null));

                return Result<Unit>.Ok(Unit.Value);
            }
            catch (Exception ex)
            {
                return Result<Unit>.Fail($"Error al desvincular comentarios: {ex.Message}");
            }
        }

        public override async Task<Result<Unit>> DeleteAsync(Comment entity)
        {
            var debRes = await DeboundCommentsFromParent(entity.Id);
            return await base.DeleteAsync(entity);
        }
    }
}
