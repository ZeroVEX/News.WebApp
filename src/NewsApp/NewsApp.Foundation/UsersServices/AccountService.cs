using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.Interfaces;
using NewsApp.Foundation.UsersServices.Constants;
using NewsApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace NewsApp.Foundation.UsersServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _singInManager;
        private readonly INewsUnitOfWork _unitOfWork;


        public AccountService(UserManager<User> userManager, SignInManager<User> singInManager, INewsUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _unitOfWork = unitOfWork;
        }


        public async Task<OperationResult<RegistrationError>> RegisterAsync(User user, string password)
        {
            user.RegistrationDate = DateTime.UtcNow;
            var identityResult = await _userManager.CreateAsync(user, password);
            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleNames.User);

                var isUserFirst = await _unitOfWork.UserRepository.CheckIfUserIsFirstAsync(user);
                if (isUserFirst)
                {
                    await _userManager.AddToRoleAsync(user, RoleNames.Admin);
                }

                return OperationResult<RegistrationError>.Success;
            }

            var errors = ConvertToOperationErrors(identityResult.Errors);

            return OperationResult<RegistrationError>.Failed(errors);
        }

        public async Task<OperationResult<LoginError>> LoginAsync(string email, string password)
        {
            var signInResult = await _singInManager.PasswordSignInAsync(email, password, false, false);

            return signInResult.Succeeded ?
                OperationResult<LoginError>.Success : OperationResult<LoginError>.Failed(LoginError.InvalidLoginOrPassword);
        }

        public async Task LoginAsync(User user)
        {
            await _singInManager.SignInAsync(user, false);
        }

        public async Task LogoutAsync()
        {
            await _singInManager.Context.SignOutAsync(IdentityConstants.ApplicationScheme);
        }

        public async Task<bool> CheckIfEmailIsUsedAsync(string email)
        {
            var user = await _userManager.FindByNameAsync(email);

            return user != null;
        }


        private static IReadOnlyCollection<RegistrationError> ConvertToOperationErrors(IEnumerable<IdentityError> identityErrors)
        {
            var operationErrors = identityErrors.Select(e => ParseError(e.Code)).ToList();

            return operationErrors;
        }

        private static RegistrationError ParseError(string value)
        {
            return value switch
            {
                ValidationErrorCodes.InvalidEmail => RegistrationError.InvalidEmail,
                ValidationErrorCodes.InvalidUserName => RegistrationError.InvalidEmail,
                ValidationErrorCodes.PasswordTooLong => RegistrationError.PasswordTooLong,
                ValidationErrorCodes.PasswordTooShort => RegistrationError.PasswordTooShort,
                ValidationErrorCodes.DisplayNameTooLong => RegistrationError.DisplayNameTooLong,
                ValidationErrorCodes.DuplicateUserName => RegistrationError.DuplicateEmail,
                ValidationErrorCodes.DuplicateEmail => RegistrationError.DuplicateEmail,
                _ => RegistrationError.UnknownError
            };
        }
    }
}