using Repositories.Interfaces;

namespace NewsApp.Repositories.Interfaces
{
    public interface INewsUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IRoleRepository RoleRepository { get; }

        INewsRepository NewsRepository { get; }
    }
}