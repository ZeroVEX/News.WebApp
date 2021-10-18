using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected DbContext DbContext { get; }


        public EntityRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }


        public void Create(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IReadOnlyCollection<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities.ToList());
        }

        public async Task<TEntity> GetByIdAsync(params object[] idValues)
        {
            var entity = await DbContext.Set<TEntity>().FindAsync(idValues);

            return entity;
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
        {
            return await DbContext.Set<TEntity>().ToListAsync();
        }
    }
}