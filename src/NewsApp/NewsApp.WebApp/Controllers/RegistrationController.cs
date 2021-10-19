using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.Interfaces;
using NewsApp.Foundation.UsersServices;
using NewsApp.WebApp.ViewModels.Account;
using Microsoft.Extensions.Localization;

namespace NewsApp.WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IStringLocalizer<RegistrationController> _localizer;


        public RegistrationController(IAccountService accountService, IStringLocalizer<RegistrationController> localizer)
        {
            _accountService = accountService;
            _localizer = localizer;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                Email = model.Email,
                DisplayName = model.DisplayName
            };
            var registrationResult = await _accountService.RegisterAsync(user, model.Password);

            if (registrationResult.IsSuccessful)
            {
                await _accountService.LoginAsync(user);

                return RedirectToAction("Index", "HomePage");
            }

            foreach (var error in registrationResult.Errors)
            {
                var (key, message) = GetErrorMessage(error);
                ModelState.AddModelError(key, message);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var isEmailUsed = await _accountService.CheckIfEmailIsUsedAsync(email);

            return Json(!isEmailUsed);
        }


        private (string modelPropertyName, string message) GetErrorMessage(RegistrationError value)
        {
            return value switch
            {
                RegistrationError.InvalidEmail => (nameof(RegisterViewModel.Email), _localizer["InvalidEmail"]),
                RegistrationError.DuplicateEmail => (nameof(RegisterViewModel.Email), _localizer["DuplicateEmail"]),
                RegistrationError.PasswordTooShort => (nameof(RegisterViewModel.Password), _localizer["PasswordTooShort"]),
                RegistrationError.PasswordTooLong => (nameof(RegisterViewModel.Password), _localizer["PasswordTooLong"]),
                RegistrationError.DisplayNameTooLong => (nameof(RegisterViewModel.DisplayName), _localizer["DisplayNameTooLong"]),
                _ => ("", "Unknown error")
            };
        }
    }
}
