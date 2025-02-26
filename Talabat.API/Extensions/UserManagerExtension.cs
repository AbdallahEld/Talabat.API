﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entites.Identity;

namespace Talabat.API.Extensions
{
	public static class UserManagerExtension
	{
		public static async Task<AppUser?> FindUserWithAdressAsync (this UserManager<AppUser> userManager, ClaimsPrincipal User)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await userManager.Users.Include(U => U.Adress).FirstOrDefaultAsync(U => U.Email == email);
			return user;
		}
	}
}
