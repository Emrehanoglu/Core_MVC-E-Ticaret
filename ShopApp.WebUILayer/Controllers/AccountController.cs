using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUILayer.Identity;
using ShopApp.WebUILayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<ApplicationUser> _userManager;
		private SignInManager<ApplicationUser> _signInManager;
		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Register()
		{
			return View(new RegisterModel());
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = new ApplicationUser
			{
				UserName = model.UserName,
				Email = model.Mail,
				FullName = model.FullName
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				return RedirectToAction("Account", "Login");
			}

			ModelState.AddModelError("", "Yanlış giriş gercekleştirildi, lütfen kontrol ediniz");

			return View(model);
		}
		public IActionResult Login()
		{
			return View(new LoginModel());
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model, string returnUrl=null)
		{
			returnUrl = returnUrl ?? "/Home/Index";
			//kullanıcı, yetkisi olmayan bir sayfaya giderse returnUrl içi dolu gelecek, 
			//eğer null gelirse /Home/Index 'e gidecek

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByNameAsync(model.UserName);
			if(user == null)
			{
				ModelState.AddModelError("", "Hatalı Kullanıcı Girişi");
				return View(model);
			}

			var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,false,false);
			//ilk false, tarayıcı kapandığında login olan kullanıcı bilgileri kalmasın anlamına gelir
			//ikinci false, kullanıcı startup içerisinde belirtilen 5 adet yanlıs giriş sonrası kitlenmeyecej

			if (result.Succeeded)
			{
				return Redirect(returnUrl);
			}

			return View(model);
		}
	}
}
