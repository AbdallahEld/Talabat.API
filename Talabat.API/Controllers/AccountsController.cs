using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services;

namespace Talabat.API.Controllers
{
	public class AccountsController : ApiBaseController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountsController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager , ITokenService tokenService , IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
		}
        // Register
        [HttpPost("Register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			if (CheckEmailExist(model.Email).Result.Value)
			{
				return BadRequest(new ApiResponse(400, "Email Already Exist"));
			}
			var User = new AppUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				UserName = model.Email.Split("@")[0]
			};
			var Result = await _userManager.CreateAsync(User , model.Password);
			if (!Result.Succeeded)
			{
				return BadRequest(new ApiResponse(400));
			}
			var ReturnedUser = new UserDto()
			{
				DisplayName = User.DisplayName,
				Email = User.Email,
				Token = await _tokenService.CreateTokenAsync(User, _userManager)
			};
			return Ok(ReturnedUser);
		}
		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var User = await _userManager.FindByEmailAsync(model.Email);
			if(User is null)
			{
				return Unauthorized(new ApiResponse(401));
			}
			var Result = await _signInManager.CheckPasswordSignInAsync(User , model.Password , false);
			if (!Result.Succeeded)
			{
				return Unauthorized(new ApiResponse(401));
			}
			var ReturnedUser = new UserDto()
			{
				DisplayName = User.DisplayName,
				Email = User.Email,
				Token = await _tokenService.CreateTokenAsync(User, _userManager)
			};
			return Ok(ReturnedUser);
		}
		[Authorize]
		[HttpGet("GetCurrentUser")]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var Email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(Email);
			var ReturnedUser = new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			};
			return Ok(ReturnedUser);
		}
		[Authorize]
		[HttpGet("Adress")]
		public async Task<ActionResult<Adress>> GetUserAdress()
		{
			//var Email = User.FindFirstValue(ClaimTypes.Email);
			//var user = await _userManager.FindByEmailAsync(Email);
			var user = await _userManager.FindUserWithAdressAsync(User);
			var MappedAdress = _mapper.Map<Adress, AdressDto>(user.Adress);
			return Ok(MappedAdress);
		}
		[Authorize]
		[HttpPut("Adress")] 
		public async Task<ActionResult<AdressDto>> UpdateAdress(AdressDto UpdatedAdress)
		{
			var user = await _userManager.FindUserWithAdressAsync(User);
			var MappedAdress = _mapper.Map<AdressDto, Adress>(UpdatedAdress);
			MappedAdress.Id = user.Adress.Id;
			user.Adress = MappedAdress;
			var Result = await _userManager.UpdateAsync(user);
			if (!Result.Succeeded)
			{
				return BadRequest(new ApiResponse(400));
			}
			return Ok(UpdatedAdress);
		}
		[HttpGet("CheckEmail")]
		public async Task<ActionResult<bool>> CheckEmailExist(string Email)
		{
			return await _userManager.FindByEmailAsync(Email) is not null;
		}
	}
}
