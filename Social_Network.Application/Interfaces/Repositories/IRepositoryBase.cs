using Social_Network.Core.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity, TId> : IServiceMarker where TEntity : BaseEntity<TId>
    {
        Task<Result<TEntity>> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        //Task<IEnumerable<TEntity>> GetActivesAsync();
        Task<Result<TEntity>> GetById(TId id);
        Task<Result<TEntity>> UpdateAsync(TEntity entity);
        Task<Result<Unit>> DeleteAsync(TEntity entity);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> AsQuery();

    }
}
