using System.Collections.Generic;

namespace NewsApp.DomainModel
{
    public class Role
    {
        public const int NameMaxLength = 256;
        public const int NormalizedNameMaxLength = 256;


        public int Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}