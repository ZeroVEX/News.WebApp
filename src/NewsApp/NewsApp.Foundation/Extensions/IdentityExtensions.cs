using System;
using System.Security.Claims;

namespace NewsApp.Foundation.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            return Convert.ToInt32(claim);
        }
    }
}