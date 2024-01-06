using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUILayer.EmailServices;
using ShopApp.WebUILayer.Identity;
//using ShopApp.DataAccess.Concrete.Memory;
using ShopApp.WebUILayer.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//IdentityConnection
			services.AddDbContext<ApplicationIdentityDbContext>(options =>
			options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

			services.AddIdentity<ApplicationUser, IdentityRole>() //Identity yapýsýný uygulamaya tanýttým,
																  //IdentityRole sýnýfýnda kullanacaðým içim ikinci parametre olarak verdim
				.AddEntityFrameworkStores<ApplicationIdentityDbContext>() //Datalarýn nerede saklanacaðýný belirttik
				.AddDefaultTokenProviders(); //Þifre deðiþtirme, mail deðiþtirme gibi iþlemlerde benzersiz bir token göndermek için kullanýlýr

			services.Configure<IdentityOptions>(options =>
			{
				//password
				options.Password.RequireDigit = true; //parola içerisinde sayý olmalý
				options.Password.RequireLowercase = true; //parola içerisinde küçük harf olmalý
				options.Password.RequiredLength = 6; //minimum 6 karakterlik bir parola olacak
				options.Password.RequireNonAlphanumeric = true; //Alphanumeric bir karakter olmak zorunda deðil
				options.Password.RequireUppercase = true; //parola içerisinde büyük harf olmalý
				
				options.Lockout.MaxFailedAccessAttempts = 5; //kullanýcý 5 defa yanlýs giriþ yapabilir
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //5 yanlýs giriþten sonra kulalnýcý 5dk kitlendi
				options.Lockout.AllowedForNewUsers = true; //kilitleme iþlemi yeni kullanýcý için de geçerli olacak

				options.User.RequireUniqueEmail = true; //ayný mail adresi ile baþka üyelik oluþturmaz

				options.SignIn.RequireConfirmedEmail = false; //kullanýcýnýn login olabilmesi için mail onayýný þart tutmaz
				options.SignIn.RequireConfirmedPhoneNumber = false; //kullanýcýnýn login olabilmesi için telefon onayýný þart tutmaz
			});

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login"; //kullanýcý giriþ yaparkenki controller/action hedefi
				options.LogoutPath = "/Account/Logout"; //kullanýcý çýkýþ yaparkenki controller/action hedefi
				options.AccessDeniedPath = "/Account/AccessDenied"; //kullanýcý yetkisi olmayan bir yere girdiði zamanki controller/action hedefi
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //tarayýcý üzerinde 60dk boyunca cookie saklanýr
				options.SlidingExpiration = true; //kullanýcýnýn hareketsiz kalma süresi ne ise tekrar sýfýrlanýr ve tekrar login olma iþlemi istenmez
				options.Cookie = new CookieBuilder
				{
					HttpOnly = true,
					Name = ".ShopApp.Security.Cookie",
					SameSite = SameSiteMode.Strict
				};
			});
			
			//IdentityConnection end

			services.AddScoped<IProductDal, EfCoreProductDal>();
			services.AddScoped<IProductService, ProductManager>();
			services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
			services.AddScoped<ICategoryService, CategoryManager>();

			//services.AddTransient<IEmailSender, EmailSender>();

			services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env,
			UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				SeedDatabase.Seed();
			}

			app.UseStaticFiles(); //wwwroot 'u dýþarýya açtým.
			app.CustomStaticFiles(); //node_modules 'u burada dýþarý actým.
			app.UseAuthentication();
			//app.UseMvcWithDefaultRoute();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "adminProducts",
					template: "Admin/Products",
					defaults: new { controller = "Admin", action = "Index" }
				);
				routes.MapRoute(
					name: "adminProducts",
					template: "Admin/Products/{id?}",
					defaults: new { controller = "Admin", action = "Edit" }
				);
				routes.MapRoute(
					name: "products",
					template: "products/{category?}",
					defaults: new { controller="Shop", action="List"}
				);
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}"
				);
			});

			//async metot olduðu için sonunda Wait eklendi
			SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();
		}
	}
}
