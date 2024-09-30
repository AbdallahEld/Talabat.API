using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
	public static class AppIdentityDbContextSeed
	{
		public static async Task SeedUserAsync(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var User = new AppUser()
				{
					DisplayName = "Abdallah Eldesouky",
					Email = "abdallaheldesoky301@gmail.com",
					UserName = "abdallaheldesouky.route",
					PhoneNumber = "01016722403"
				};
				await userManager.CreateAsync(User, "P@s$w0rd");
			}
		}
	}
}
