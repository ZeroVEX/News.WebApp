using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Interfaces;
using NewsApp.DomainModel;

namespace NewsApp.Repositories.Interfaces
{
    public interface INewsRepository : IRepository<News>
    {
        Task<int> CountNewsAsync(string titleFilter);

        Task<IReadOnlyCollection<News>> GetNewsPageAsync(int skip, int take, string titleFilter);

        Task<IReadOnlyCollection<News>> GetByCreatorIdAsync(int creatorId);
    }
}