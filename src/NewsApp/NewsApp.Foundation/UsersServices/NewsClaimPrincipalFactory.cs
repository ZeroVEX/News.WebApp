using System.Security.Claims;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.UsersServices.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace NewsApp.Foundation.UsersServices
{
    public class NewsClaimPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public NewsClaimPrincipalFactory(UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {

        }


        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim(NewsClaimTypes.DisplayName, user.DisplayName));

            return identity;
        }
    }
}