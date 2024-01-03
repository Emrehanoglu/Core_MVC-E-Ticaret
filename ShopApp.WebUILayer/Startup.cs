using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
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
			//IdentityConnection end

			services.AddScoped<IProductDal, EfCoreProductDal>();
			services.AddScoped<IProductService, ProductManager>();
			services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
			services.AddScoped<ICategoryService, CategoryManager>();
			services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
		}
	}
}
