using System;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.UsersServices.Constants;
using Microsoft.AspNetCore.Identity;

namespace NewsApp.Foundation.UsersServices.Validators
{
    public class NewsPasswordValidator : IPasswordValidator<User>
    {
        public virtual Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            IdentityError error = null;

            if (password.Length > User.PasswordMaxLength)
            {
                error = new IdentityError
                {
                    Code = ValidationErrorCodes.PasswordTooLong
                };
            }

            var validationResult = error == null ? IdentityResult.Success : IdentityResult.Failed(error);

            return Task.FromResult(validationResult);
        }
    }
}