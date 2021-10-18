using System.ComponentModel.DataAnnotations;
using NewsApp.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace NewsApp.WebApp.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(DomainModel.User.EmailMaxLength)]
        [EmailAddress]
        [Remote(nameof(RegistrationController.CheckEmail), "Registration", ErrorMessage = "Duplicate Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(DomainModel.User.DisplayNameMaxLength)]
        [Display(Name = "User name")]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(DomainModel.User.PasswordMaxLength, MinimumLength = DomainModel.User.PasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(DomainModel.User.PasswordMaxLength)]
        [Compare(nameof(Password), ErrorMessage = "Passwords are not the same")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}