using System.ComponentModel.DataAnnotations;
using NewsApp.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace NewsApp.WebApp.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "EmailRequired")]
        [StringLength(DomainModel.User.EmailMaxLength, ErrorMessage = "MaxLength")]
        [EmailAddress(ErrorMessage = "EmailAddress")]
        [Remote(nameof(RegistrationController.CheckEmail), "Registration", ErrorMessage = "DuplicateEmail")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "DisplayNameRequired")]
        [StringLength(DomainModel.User.DisplayNameMaxLength, ErrorMessage = "MaxLength")]
        [Display(Name = "UserName")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(DomainModel.User.PasswordMaxLength, MinimumLength = DomainModel.User.PasswordMinLength, ErrorMessage = "MaxPasswordLength")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPasswordRequired")]
        [StringLength(DomainModel.User.PasswordMaxLength, ErrorMessage = "MaxLength")]
        [Compare(nameof(Password), ErrorMessage = "ComparePasswords")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        public string PasswordConfirm { get; set; }
    }
}