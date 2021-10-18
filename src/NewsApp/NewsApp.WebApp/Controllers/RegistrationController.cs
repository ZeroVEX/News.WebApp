using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.Interfaces;
using NewsApp.Foundation.UsersServices;
using NewsApp.WebApp.ViewModels.Account;

namespace NewsApp.WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IAccountService _accountService;


        public RegistrationController(IAccountService accountService)
        {
            _accountService = accountService;
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


        private static (string modelPropertyName, string message) GetErrorMessage(RegistrationError value)
        {
            return value switch
            {
                RegistrationError.InvalidEmail => (nameof(RegisterViewModel.Email), "Invalid email"),
                RegistrationError.DuplicateEmail => (nameof(RegisterViewModel.Email), "Email is already used"),
                RegistrationError.PasswordTooShort => (nameof(RegisterViewModel.Password), "Password too short"),
                RegistrationError.PasswordTooLong => (nameof(RegisterViewModel.Password), "Password too long"),
                RegistrationError.DisplayNameTooLong => (nameof(RegisterViewModel.DisplayName), "Display name too long"),
                _ => ("", "Unknown error")
            };
        }
    }
}
