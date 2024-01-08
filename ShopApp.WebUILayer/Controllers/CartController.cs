using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.WebUILayer.Identity;
using ShopApp.WebUILayer.Models;
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
			return View(new CartModel()
			{
				CartId = cart.Id,
				CartItems = cart.CartItems.Select(x => new CartItemModel()
				{
					CartItemId = x.Id,
					ProductId = x.Product.Id,
					ImageUrl = x.Product.ImageUrl,
					Name = x.Product.Name,
					Price = (decimal)x.Product.Price,
					Quantity = x.Quantity
				}).ToList()
			});
		}
		[HttpPost]
		public IActionResult AddToCart(int productId, int quantity)
		{
			_cartService.AddToCart(_userManager.GetUserId(User), productId, quantity);
			return RedirectToAction("Index");
		}
		[HttpPost]
		public IActionResult DeleteFromCart(int productId)
		{
			_cartService.DeleteFromCart(_userManager.GetUserId(User), productId);
			return RedirectToAction("Index");
		}
		public IActionResult Checkout()
		{
			return View();
		}
	}
}
