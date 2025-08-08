using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.Interfaces.Repositories;
using Social_Network.Core.Domain.Base;
using Social_Network.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Infraestructure.Persistence.Repositories
{
    public class BaseRepository<TEntity, TKey> : IRepositoryBase<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected readonly DbSet<TEntity> _entitySet;
        protected readonly SocialNetworkContext _context;

        protected BaseRepository(SocialNetworkContext context)
        {
            _context = context;
            _entitySet = _context.Set<TEntity>();
        }


        public async virtual Task<Result<TEntity>> AddAsync(TEntity entity)
        {
            try
            {
                _entitySet.Add(entity);
                await _context.SaveChangesAsync();
                return Result<TEntity>.Ok(entity);
            }
            catch (Exception ex)
            {
                return Result<TEntity>.Fail(ex.Message);
            }
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
            => await _entitySet.AnyAsync(predicate);

        public IQueryable<TEntity> AsQuery()
            => _entitySet.AsQueryable();

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
            => await _entitySet.CountAsync(predicate);

        public async virtual Task<Result<Unit>> DeleteAsync(TEntity entity)
        {
            try
            {

                _entitySet.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<Unit>.Ok(Unit.Value);
            }
            catch (Exception ex)
            {
                return Result<Unit>.Fail(ex.Message);
            }
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
            => await _entitySet.FirstOrDefaultAsync(predicate);



        public async Task<IEnumerable<TEntity>> GetAllAsync()
             => await _entitySet.ToListAsync();

        public async Task<Result<TEntity>> GetById(TKey id)
        {
            var entity = await _entitySet.FindAsync(id);
            if (entity == null)
            {
                return Result<TEntity>.Fail($"Entity with id {id} not found.");
            }
            return Result<TEntity>.Ok(entity);
        }

        public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                _entitySet.Update(entity);
                await _context.SaveChangesAsync();
                return Result<TEntity>.Ok(entity);
            }
            catch (Exception ex)
            {
                return Result<TEntity>.Fail(ex.Message);
            }
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
            => await _entitySet.Where(predicate).ToListAsync();
    }
}
