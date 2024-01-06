using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Identity
{
	public static class SeedIdentity
	{
		//burası Startup tarafında cağırıldı.
		public static async Task Seed(UserManager<ApplicationUser> userManager, 
			RoleManager<IdentityRole> roleManager,IConfiguration configuration)
		{
			//IConfiguration configuration ile de appsettings.json tarafındaki kısma erişiyorum
			var username = configuration["Data:AdminUser:username"];
			var email = configuration["Data:AdminUser:email"];
			var password = configuration["Data:AdminUser:password"];
			var role = configuration["Data:AdminUser:role"];

			if(await userManager.FindByNameAsync(username) == null)
			{
				//rol oluşturuldu
				await roleManager.CreateAsync(new IdentityRole(role));

				var user = new ApplicationUser
				{
					UserName = username,
					Email = email,
					FullName = "Amin User",
					EmailConfirmed = true //email onaylanmış gözükecek, tekrar email onayı istemeyecek
				};
				//admin user oluşturuldu  
				var result = await userManager.CreateAsync(user, password);
				if (result.Succeeded)
				{
					//kullanıcıya admin rolünün ataması yapıldı
					await userManager.AddToRoleAsync(user, role);
				}
			}
		}
	}
}
