using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Interfaces;
using NewsApp.DomainModel;

namespace NewsApp.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> CheckIfUserIsFirstAsync(User user);

        Task<int> CountUsersAsync(string nameFilter);

        Task<IReadOnlyCollection<User>> GetUsersPageAsync(int skip, int take, string nameFilter);

        Task<User> GetByEmailAsync(string email);

        Task<IReadOnlyCollection<string>> GetRolesAsync(int userId);

        Task<User> GetUserByIdAsync(int id);
    }
}