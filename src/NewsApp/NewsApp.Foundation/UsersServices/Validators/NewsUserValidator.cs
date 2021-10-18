using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.UsersServices.Constants;
using Microsoft.AspNetCore.Identity;

namespace NewsApp.Foundation.UsersServices.Validators
{
    public class NewsUserValidator : IUserValidator<User>
    {
        public virtual Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var errors = new List<IdentityError>();

            if (user.DisplayName.Length > User.DisplayNameMaxLength)
            {
                errors.Add(new IdentityError
                {
                    Code = ValidationErrorCodes.DisplayNameTooLong
                });
            }
            if (user.Email.Length > User.EmailMaxLength)
            {
                errors.Add(new IdentityError
                {
                    Code = ValidationErrorCodes.InvalidEmail
                });
            }
            else if (!new EmailAddressAttribute().IsValid(user.Email))
            {
                errors.Add(new IdentityError
                {
                    Code = ValidationErrorCodes.InvalidEmail
                });
            }

            var validationResult = errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;

            return Task.FromResult(validationResult);
        }
    }
}