using System.Linq;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.Interfaces;
using NewsApp.Foundation.UsersServices;
using NewsApp.WebApp.ViewModels;
using NewsApp.WebApp.ViewModels.Pagination;
using NewsApp.WebApp.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace NewsApp.WebApp.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public class UsersController : Controller
    {
        private const int UsersPageSize = 2;

        private readonly IUserManagementService _userManagementService;
        private readonly IStringLocalizer<UsersController> _localizer;


        public UsersController(IUserManagementService userManagementService, IStringLocalizer<UsersController> localizer)
        {
            _userManagementService = userManagementService;
            _localizer = localizer;
        }


        public async Task<IActionResult> Index(string filter, int page = 1)
        {
            var userPage = await _userManagementService.GetUserPageAsync((page - 1) * UsersPageSize, UsersPageSize, filter);

            var userViewModels = userPage.Items.Select(u => new UserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                DisplayName = u.DisplayName,
                RegistrationDate = u.RegistrationDate,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            }).ToList();

            var pageViewModel = new ItemsPageViewModel<UserViewModel>(userViewModels, userPage.Count, page, UsersPageSize);
            var userManagementViewModel = new ItemsSearchViewModel<UserViewModel>(pageViewModel, filter);

            return View(userManagementViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManagementService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = user.UserRoles.Select(r => r.Role.Name).ToList();

            var editUserViewModel = new EditUserViewModel
            {
                DisplayName = user.DisplayName,
                UserRoles = userRoles,
            };
            await InitializeViewModelAsync(editUserViewModel);

            return View(editUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await InitializeViewModelAsync(model);
                return View(model);
            }

            var updateUserResult = await _userManagementService.UpdateUserAsync(model.Id, model.UserRoles, model.DisplayName);

            if (updateUserResult.IsSuccessful)
            {
                return RedirectToAction("Index");
            }

            await InitializeViewModelAsync(model);

            foreach (var error in updateUserResult.Errors)
            {
                var (key, message) = GetErrorMessage(error);
                ModelState.AddModelError(key, message);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManagementService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManagementService.DeleteUserAsync(user);

            return RedirectToAction("Index");
        }


        private async Task InitializeViewModelAsync(EditUserViewModel viewModel)
        {
            viewModel.AllRoles = await _userManagementService.GetAllRolesAsync();
        }

        private (string modelPropertyName, string message) GetErrorMessage(UpdateUserError value)
        {
            return value switch
            {
                UpdateUserError.DisplayNameIsEmpty => (nameof(EditUserViewModel.DisplayName), _localizer["DisplayNameIsEmpty"]),
                UpdateUserError.DisplayNameIsTooLong => (nameof(EditUserViewModel.DisplayName), _localizer["DisplayNameIsTooLong"]),
                UpdateUserError.EmptyRoles => (nameof(EditUserViewModel.UserRoles), _localizer["EmptyRoles"]),
                _ => ("", "Unknown error")
            };
        }
    }
}
