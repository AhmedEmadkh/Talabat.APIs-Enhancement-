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
        private readonly SignInManager<AppUser> _signInManager;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);

            if(User is null)
                return BadRequest(new ApiResponse((int)HttpStatusCode.Unauthorized));

            var Result = await _signInManager.CheckPasswordSignInAsync(User,model.Password,false);
            if(!Result.Succeeded)
                return Unauthorized(new ApiResponse((int)HttpStatusCode.Unauthorized));

            var ReturnedUser = new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = model.Email,
                Token = "This will be Token"
            };
            return Ok(ReturnedUser);
        }
    }
}
