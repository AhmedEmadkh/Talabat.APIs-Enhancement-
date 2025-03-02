using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identitiy;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Identity;
using Talabat.Services;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services,IConfiguration configuration)
        {
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddIdentity<AppUser, IdentityRole>()
                 .AddEntityFrameworkStores<AppIdentityDbContext>();
            Services.AddAuthentication(Options => // UserManager - RoleManager - SignInManager 
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                     .AddJwtBearer(Options =>
                     {
                         Options.TokenValidationParameters = new TokenValidationParameters()
                         {
                             ValidateIssuer = true,
                             ValidIssuer = configuration["JWT:ValidIssuer"],
                             ValidateAudience = true,
                             ValidAudience = configuration["JWT:ValidAudience"],
                             ValidateLifetime = true,
                             ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:ExpirationInDays"])),
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                         };
                     }); 


            return Services;
        }
    }
}
