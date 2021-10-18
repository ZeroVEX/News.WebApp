using System;
using System.Collections.Generic;

namespace NewsApp.WebApp.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public IReadOnlyCollection<string> Roles { get; set; }
    }
}