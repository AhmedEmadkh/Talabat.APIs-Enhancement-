using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identitiy;
using Talabat.Repository.Identity;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            Services.AddAuthentication(); // UserManager - RoleManager - SignInManager 
            Services.AddIdentity<AppUser, IdentityRole>()
                             .AddEntityFrameworkStores<AppIdentityDbContext>();

            return Services;
        }
    }
}
