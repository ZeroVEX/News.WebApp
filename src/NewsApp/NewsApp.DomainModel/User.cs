using System;
using System.Collections.Generic;

namespace NewsApp.DomainModel
{
    public class User
    {
        public const int EmailMaxLength = 256;
        public const int NormalizedEmailMaxLength = 256;
        public const int DisplayNameMaxLength = 256;
        public const int PasswordMinLength = 4;
        public const int PasswordMaxLength = 256;


        public int Id { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public string DisplayName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string PasswordHash { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public List<News> News { get; set; }
    }
}