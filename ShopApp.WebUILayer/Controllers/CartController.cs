using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUILayer.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private ICartService _cartService;
		private UserManager<ApplicationUser> _userManager;
		public CartController(ICartService cartService, UserManager<ApplicationUser> userManager)
		{
			_cartService = cartService;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
			return View(cart);
		}
		[HttpPost]
		public IActionResult AddToCart()
		{
			return View();
		}
	}
}
