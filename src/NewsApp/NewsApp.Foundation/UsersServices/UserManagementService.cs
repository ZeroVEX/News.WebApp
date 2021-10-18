using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.Interfaces;
using NewsApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace NewsApp.Foundation.UsersServices
{
    public class UserManagementService : IUserManagementService
    {
        private readonly INewsUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;


        public UserManagementService(INewsUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public async Task<OperationResult<UpdateUserError>> UpdateUserAsync(int userId, IReadOnlyCollection<string> newRoles, string newDisplayName)
        {
            var errors = new List<UpdateUserError>();
            if (string.IsNullOrWhiteSpace(newDisplayName))
            {
                errors.Add(UpdateUserError.DisplayNameIsEmpty);
            }
            else if (newDisplayName.Length > User.DisplayNameMaxLength)
            {
                errors.Add(UpdateUserError.DisplayNameIsTooLong);
            }

            if (newRoles == null || newRoles.Count == 0)
            {
                errors.Add(UpdateUserError.EmptyRoles);
            }

            if (errors.Count != 0)
            {
                return OperationResult<UpdateUserError>.Failed(errors);
            }

            var user = await GetUserByIdAsync(userId);
            user.DisplayName = newDisplayName;

            var userRoles = user.UserRoles.Select(r => r.Role.Name).ToList();
            // ReSharper disable once AssignNullToNotNullAttribute
            var addedRoles = newRoles.Except(userRoles);
            var removedRoles = userRoles.Except(newRoles);

            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveAsync();

            return OperationResult<UpdateUserError>.Success;
        }

        public async Task DeleteUserAsync(User user)
        {
            var news = await _unitOfWork.NewsRepository.GetByCreatorIdAsync(user.Id);
            foreach (var item in news)
            {
                item.CreatorId = null;
                _unitOfWork.NewsRepository.Update(item);
            }

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<EntityPage<User>> GetUserPageAsync(int skip, int take, string filter)
        {
            var users = await _unitOfWork.UserRepository.GetUsersPageAsync(skip, take, filter);
            var count = await _unitOfWork.UserRepository.CountUsersAsync(filter);
            var userPage = new EntityPage<User>(users, count);

            return userPage;
        }

        public async Task<IReadOnlyCollection<Role>> GetAllRolesAsync()
        {
            var allRoles = await _unitOfWork.RoleRepository.GetAllAsync();

            return allRoles;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetUserByIdAsync(id);
        }
    }
}