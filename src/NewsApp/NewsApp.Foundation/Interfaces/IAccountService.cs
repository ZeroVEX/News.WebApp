using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.UsersServices;

namespace NewsApp.Foundation.Interfaces
{
    public interface IAccountService
    {
        Task<OperationResult<RegistrationError>> RegisterAsync(User user, string password);

        Task<OperationResult<LoginError>> LoginAsync(string email, string password);

        Task LoginAsync(User user);

        Task LogoutAsync();

        Task<bool> CheckIfEmailIsUsedAsync(string email);
    }
}