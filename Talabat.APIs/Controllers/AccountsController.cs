using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Talabat.APIs.DTOs.Accounts;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identitiy;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        // Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split('@')[0],
            };

           var Result = await _userManager.CreateAsync(User, model.Password);

            if (!Result.Succeeded)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest));

            var ReturnedUser = new UserDTO()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = "This will be Token"
            };

            return Ok(ReturnedUser);
        }

        // Login
    }
}
