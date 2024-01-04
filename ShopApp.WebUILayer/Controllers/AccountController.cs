using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
				//generate token
				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				var callbackUrl = Url.Action("ConfirmEmail", "Account", new
				{
					userId = user.Id,
					token = code
				});

				return Redirect(callbackUrl);
			}

			ModelState.AddModelError("", "Yanlış giriş gercekleştirildi, lütfen kontrol ediniz");

			return View(model);
		}
		public IActionResult Login(string ReturnUrl=null)
		{
			return View(new LoginModel() { 
				ReturnUrl = ReturnUrl
			});
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByEmailAsync(model.Email);
			if(user == null)
			{
				ModelState.AddModelError("", "Hatalı Email Bilgisi");
				return View(model);
			}

			var result = await _signInManager.PasswordSignInAsync(user, model.Password,false,false);
			//ilk false, tarayıcı kapandığında login olan kullanıcı bilgileri kalmasın anlamına gelir
			//ikinci false, kullanıcı startup içerisinde belirtilen 5 adet yanlıs giriş sonrası kitlenmeyecej

			if (result.Succeeded)
			{
				return Redirect(model.ReturnUrl ?? "/Home/Index");
			}

			return View(model);
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Redirect("/Home/Index");
		}
		public async Task<IActionResult> ConfirmEmail(string userId, string token)
		{
			if (userId==null || token == null)
			{
				TempData["messages"] = "Geçersiz Token Bilgisi!";
				return View();
			}

			var user = await _userManager.FindByIdAsync(userId);
			if(user == null)
			{
				TempData["messages"] = "Böyle bir kullanıcı bulunmamaktadır.";
				return View();
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);
			if (result.Succeeded)
			{
				TempData["messages"] = "Hesabınız Onaylandı.";
				return View();
			}

			TempData["messages"] = "Onaylanmamış Hesap!";
			return View();
		}
		public IActionResult ForgotPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			if(email == null)
			{
				return View();
			}

			var user = await _userManager.FindByEmailAsync(email);
			if(user == null)
			{
				return View();
			}

			var code = await _userManager.GeneratePasswordResetTokenAsync(user);
			var callbackUrl = Url.Action("ResetPassword", "Account", new
			{
				userId = user.Id,
				token = code
			});

			return Redirect(callbackUrl);
		}
		public IActionResult ResetPassword()
		{
			return View();
		}
	}
}
