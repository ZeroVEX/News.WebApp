using Repositories.Repositories;
using NewsApp.DomainModel;
using NewsApp.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Repositories.Implementation
{
    public class NewsRepository : EntityRepository<News>, INewsRepository
    {
        public NewsRepository(DbContext dbContext)
            : base(dbContext)
        {

        }


        public async Task<int> CountNewsAsync(string titleFilter)
        {
            var news = DbContext.Set<News>();

            var count = titleFilter == null
                ? await news.CountAsync()
                : await news.CountAsync(n => n.Title.Contains(titleFilter));

            return count;
        }

        public async Task<IReadOnlyCollection<News>> GetNewsPageAsync(int skip, int take, string titleFilter)
        {
            var newsQuery = DbContext.Set<News>().Include(n => n.Creator).AsQueryable();

            if (titleFilter != null)
            {
                newsQuery = newsQuery.Where(n => n.Title.Contains(titleFilter));
            }

            var news = await newsQuery
                .OrderBy(n => n.Title)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return news;
        }

        public async Task<IReadOnlyCollection<News>> GetByCreatorIdAsync(int creatorId)
        {
            var news = await DbContext.Set<News>()
                .Where(n => n.CreatorId == creatorId)
                .ToListAsync();

            return news;
        }
    }
}