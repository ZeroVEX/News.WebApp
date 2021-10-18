using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Interfaces;
using Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _dbContext;

        private readonly Dictionary<Type, object> _repositories;
        private readonly Dictionary<Type, Type> _registeredTypes;


        protected UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext;

            _repositories = new Dictionary<Type, object>();
            _registeredTypes = new Dictionary<Type, Type>();
        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return (IRepository<T>)repository;
            }

            if (_registeredTypes.TryGetValue(typeof(T), out var registeredType))
            {
                repository = Activator.CreateInstance(registeredType, _dbContext);
                _repositories.Add(typeof(T), repository);

                return (IRepository<T>)repository;
            }

            repository = new EntityRepository<T>(_dbContext);
            _repositories.Add(typeof(T), repository);

            return (IRepository<T>)repository;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }


        protected void RegisterRepository<TEntity, TRepository>()
            where TEntity : class
            where TRepository : class, IRepository<TEntity>
        {
            _registeredTypes.Add(typeof(TEntity), typeof(TRepository));
        }
    }
}