using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identitiy;

namespace Talabat.APIs.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindAddressOfCurrentUser(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(U => U.Address).SingleOrDefaultAsync(U => U.Email == email);

            return user;
        }
    }
}
