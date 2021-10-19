using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NewsApp.Foundation.Interfaces;
using NewsApp.WebApp.ViewModels.Account;
using Microsoft.Extensions.Localization;

namespace NewsApp.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IStringLocalizer<AccountController> _localizer;


        public AccountController(IAccountService accountService, IStringLocalizer<AccountController> localizer)
        {
            _accountService = accountService;
            _localizer = localizer;
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginResult = await _accountService.LoginAsync(model.Email, model.Password);
            if (loginResult.IsSuccessful)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "HomePage");
            }

            ModelState.AddModelError("", _localizer["LoginError"]);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

            return RedirectToAction("Register", "Registration");
        }
    }
}
