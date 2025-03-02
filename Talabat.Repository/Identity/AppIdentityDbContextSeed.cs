using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identitiy;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ahmed Emad",
                    Email = "eng.ahmed.emad.work@gmail.com",
                    UserName = "ahmed.emad",
                    PhoneNumber = "01028618665"
                };
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }
        }
    }
}
