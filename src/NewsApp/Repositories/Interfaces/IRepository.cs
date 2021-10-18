using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRepository<T>
    {
        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        void RemoveRange(IReadOnlyCollection<T> entities);

        Task<T> GetByIdAsync(params object[] idValues);

        Task<IReadOnlyCollection<T>> GetAllAsync();
    }
}