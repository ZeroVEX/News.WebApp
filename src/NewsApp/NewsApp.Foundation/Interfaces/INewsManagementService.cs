using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.NewsServices;

namespace NewsApp.Foundation.Interfaces
{
    public interface INewsManagementService
    {
        Task<OperationResult<NewsManagementError>> CreateNewsAsync(News news);

        Task<OperationResult<NewsManagementError>> UpdateNewsAsync(News news, News fromNews);

        Task DeleteNewsAsync(News news);

        Task<EntityPage<News>> GetNewsPageAsync(int skip, int take, string filter);

        Task<News> GetNewsByIdAsync(int id);
    }
}