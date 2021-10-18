using Repositories;
using NewsApp.DomainModel;
using NewsApp.Repositories.Contexts;
using NewsApp.Repositories.Implementation;
using NewsApp.Repositories.Interfaces;

namespace NewsApp.Repositories
{
    public class NewsUnitOfWork : UnitOfWork<NewsDbContext>, INewsUnitOfWork
    {
        public IUserRepository UserRepository => (IUserRepository)GetRepository<User>();

        public IRoleRepository RoleRepository => (IRoleRepository)GetRepository<Role>();

        public INewsRepository NewsRepository => (INewsRepository)GetRepository<News>();


        public NewsUnitOfWork(NewsDbContext dbContext)
            : base(dbContext)
        {
            RegisterRepository<User, UserRepository>();
            RegisterRepository<Role, RoleRepository>();
            RegisterRepository<News, NewsRepository>();
        }
    }
}