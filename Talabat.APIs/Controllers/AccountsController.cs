using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Talabat.APIs.DTOs.Accounts;
using Talabat.APIs.DTOs.Orders;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identitiy;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
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
                Token = await _tokenService.CreateTokenAsync(User,_userManager)
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
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            };
            return Ok(ReturnedUser);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var user = await _userManager.FindAddressOfCurrentUser(User);

            var MappedAddress = _mapper.Map<AddressDTO>(user.Address);
            return MappedAddress;
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO udpatedAddress)
        {
            var address = _mapper.Map<Address>(udpatedAddress);

            var user = await _userManager.FindAddressOfCurrentUser(User);

            address.Id = user.Address.Id;
            user.Address = address;

            var result = await _userManager.UpdateAsync(user);

            if(!result.Succeeded)
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));

            return Ok(udpatedAddress);
        }
    }
}
