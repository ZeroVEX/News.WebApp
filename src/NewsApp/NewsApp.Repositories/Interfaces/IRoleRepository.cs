using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Interfaces;
using NewsApp.DomainModel;

namespace NewsApp.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByNameAsync(string normalizedName);

        Task<IReadOnlyCollection<User>> GetUsersInRoleAsync(int roleId);
    }
}