using System.Collections.Generic;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.UsersServices;

namespace NewsApp.Foundation.Interfaces
{
    public interface IUserManagementService
    {
        Task<OperationResult<UpdateUserError>> UpdateUserAsync(int userId, IReadOnlyCollection<string> newRoles, string newDisplayName);

        Task DeleteUserAsync(User user);

        Task<EntityPage<User>> GetUserPageAsync(int skip, int take, string filter);

        Task<IReadOnlyCollection<Role>> GetAllRolesAsync();

        Task<User> GetUserByIdAsync(int id);
    }
}