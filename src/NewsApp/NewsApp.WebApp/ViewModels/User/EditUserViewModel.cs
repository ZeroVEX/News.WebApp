using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NewsApp.DomainModel;

namespace NewsApp.WebApp.ViewModels.User
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "NameRequired")]
        [StringLength(DomainModel.User.DisplayNameMaxLength, ErrorMessage = "MaxLength")]
        [Display(Name = "UserName")]
        public string DisplayName { get; set; }

        public IReadOnlyCollection<string> UserRoles { get; set; }

        public IReadOnlyCollection<Role> AllRoles { get; set; }
    }
}