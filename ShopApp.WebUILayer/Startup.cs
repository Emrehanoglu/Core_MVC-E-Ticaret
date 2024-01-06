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

			services.AddIdentity<ApplicationUser, IdentityRole>() //Identity yap�s�n� uygulamaya tan�tt�m,
																  //IdentityRole s�n�f�nda kullanaca��m i�im ikinci parametre olarak verdim
				.AddEntityFrameworkStores<ApplicationIdentityDbContext>() //Datalar�n nerede saklanaca��n� belirttik
				.AddDefaultTokenProviders(); //�ifre de�i�tirme, mail de�i�tirme gibi i�lemlerde benzersiz bir token g�ndermek i�in kullan�l�r

			services.Configure<IdentityOptions>(options =>
			{
				//password
				options.Password.RequireDigit = true; //parola i�erisinde say� olmal�
				options.Password.RequireLowercase = true; //parola i�erisinde k���k harf olmal�
				options.Password.RequiredLength = 6; //minimum 6 karakterlik bir parola olacak
				options.Password.RequireNonAlphanumeric = true; //Alphanumeric bir karakter olmak zorunda de�il
				options.Password.RequireUppercase = true; //parola i�erisinde b�y�k harf olmal�
				
				options.Lockout.MaxFailedAccessAttempts = 5; //kullan�c� 5 defa yanl�s giri� yapabilir
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //5 yanl�s giri�ten sonra kulaln�c� 5dk kitlendi
				options.Lockout.AllowedForNewUsers = true; //kilitleme i�lemi yeni kullan�c� i�in de ge�erli olacak

				options.User.RequireUniqueEmail = true; //ayn� mail adresi ile ba�ka �yelik olu�turmaz

				options.SignIn.RequireConfirmedEmail = false; //kullan�c�n�n login olabilmesi i�in mail onay�n� �art tutmaz
				options.SignIn.RequireConfirmedPhoneNumber = false; //kullan�c�n�n login olabilmesi i�in telefon onay�n� �art tutmaz
			});

			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login"; //kullan�c� giri� yaparkenki controller/action hedefi
				options.LogoutPath = "/Account/Logout"; //kullan�c� ��k�� yaparkenki controller/action hedefi
				options.AccessDeniedPath = "/Account/AccessDenied"; //kullan�c� yetkisi olmayan bir yere girdi�i zamanki controller/action hedefi
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //taray�c� �zerinde 60dk boyunca cookie saklan�r
				options.SlidingExpiration = true; //kullan�c�n�n hareketsiz kalma s�resi ne ise tekrar s�f�rlan�r ve tekrar login olma i�lemi istenmez
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

			app.UseStaticFiles(); //wwwroot 'u d��ar�ya a�t�m.
			app.CustomStaticFiles(); //node_modules 'u burada d��ar� act�m.
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

			//async metot oldu�u i�in sonunda Wait eklendi
			SeedIdentity.Seed(userManager, roleManager, Configuration).Wait();
		}
	}
}
