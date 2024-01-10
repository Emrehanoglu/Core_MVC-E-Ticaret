using IyzipayCore;
using IyzipayCore.Model;
using IyzipayCore.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
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
		private IOrderService _orderService;
		public CartController(ICartService cartService, UserManager<ApplicationUser> userManager, IOrderService orderService)
		{
			_cartService = cartService;
			_userManager = userManager;
			_orderService = orderService;
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
			var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
			var order = new OrderModel();
			order.CartModel = new CartModel()
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
			};
			return View(order);
		}
		[HttpPost]
		public IActionResult Checkout(OrderModel model)
		{
			//gelen modeli kontrol edelim
			if (ModelState.IsValid)
			{
				var userId = _userManager.GetUserId(User);
				var cart = _cartService.GetCartByUserId(userId);

				model.CartModel = new CartModel()
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
				};

				//odeme
				var payment = PaymentProcess(model);

				//ödeme işlemi başarılı ise Success.cshtml 'e yonlendir
				if (payment.Status == "success")
				{
					SaveOrder(model, payment, userId); //bu noktada sipariş kaydı oluşturulsun, başarılı ise veritabanına kaydedelim
					CleanCart(cart.Id.ToString()); // daha sonra sepeti temizleyelim
					return View("Success");
				}
			}
			//Ödeme işlemi başarısız ise modeli tekrar aynı sayfaya yönlendir
			return View(model);
		}

		private void SaveOrder(OrderModel model, Payment payment, string userId)
		{
			var order = new Order();

			order.OrderNumber = new Random().Next(111111, 999999).ToString(); //111111 - 999999 arasında random bir sayı
			order.OrderState = EnumOrderState.Completed;
			order.PaymentTypes = EnumPaymentTypes.CreditCart;
			order.PaymentId = payment.PaymentId;
			order.ConversationId = payment.ConversationId;
			order.OrderDate = new DateTime();
			order.FirstName = model.FirstName;
			order.LastName = model.LastName;
			order.Email = model.Email;
			order.Phone = model.Phone;
			order.Address = model.Address;
			order.UserId = userId;

			foreach(var item in model.CartModel.CartItems)
			{
				var orderitem = new OrderItem()
				{
					Price = item.Price,
					Quantity = item.Quantity,
					ProductId = item.ProductId
				};
				order.OrderItems.Add(orderitem);
			}
			_orderService.Create(order);
		}

		private void CleanCart(string cartId)
		{
			_cartService.ClearCart(cartId);
		}

		private Payment PaymentProcess(OrderModel model)
		{
			Options options = new Options();
			options.ApiKey = "sandbox-AYwplCBFjf5FgvwCdN5fTQ5T5wq4onX8";
			options.SecretKey = "sandbox-JPLNsqPOW9LB8pBdP3Sf38Y9d6Y6Rsmm";
			options.BaseUrl = "https://sandbox-api.iyzipay.com";

			CreatePaymentRequest request = new CreatePaymentRequest();
			request.Locale = Locale.TR.ToString();
			//guvenlık acısından siparis olusmadan once ve sonraki ConversationId alanı kontrol edılecek
			request.ConversationId = Guid.NewGuid().ToString();
			request.Price = model.CartModel.TotalPrice().ToString().Split(",")[0];
			request.PaidPrice = model.CartModel.TotalPrice().ToString().Split(",")[0];
			request.Currency = Currency.TRY.ToString();
			request.Installment = 1;
			request.BasketId = model.CartModel.CartId.ToString();
			request.PaymentChannel = PaymentChannel.WEB.ToString();
			request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

			PaymentCard paymentCard = new PaymentCard();
			paymentCard.CardHolderName = model.CardName;
			paymentCard.CardNumber = model.CardNumber;
			paymentCard.ExpireMonth = model.ExpirationMonth;
			paymentCard.ExpireYear = model.ExpirationYear;
			paymentCard.Cvc = model.Cvv;
			paymentCard.RegisterCard = 0;
			request.PaymentCard = paymentCard;

			//formda kart bilgileri kısmında aşağıdakileri gireceğim.
			//paymentCard.CardHolderName = "John Doe";
			//paymentCard.CardNumber = "5528790000000008";
			//paymentCard.ExpireMonth = "12";
			//paymentCard.ExpireYear = "2030";
			//paymentCard.Cvc = "123";

			Buyer buyer = new Buyer();
			buyer.Id = "BY789";
			buyer.Name = "John";
			buyer.Surname = "Doe";
			buyer.GsmNumber = "+905350000000";
			buyer.Email = "email@email.com";
			buyer.IdentityNumber = "74300864791";
			buyer.LastLoginDate = "2015-10-05 12:43:35";
			buyer.RegistrationDate = "2013-04-21 15:12:09";
			buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
			buyer.Ip = "85.34.78.112";
			buyer.City = "Istanbul";
			buyer.Country = "Turkey";
			buyer.ZipCode = "34732";
			request.Buyer = buyer;

			Address shippingAddress = new Address();
			shippingAddress.ContactName = "Jane Doe";
			shippingAddress.City = "Istanbul";
			shippingAddress.Country = "Turkey";
			shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
			shippingAddress.ZipCode = "34742";
			request.ShippingAddress = shippingAddress;

			Address billingAddress = new Address();
			billingAddress.ContactName = "Jane Doe";
			billingAddress.City = "Istanbul";
			billingAddress.Country = "Turkey";
			billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
			billingAddress.ZipCode = "34742";
			request.BillingAddress = billingAddress;

			List<BasketItem> basketItems = new List<BasketItem>();
			BasketItem basketItem;
			foreach(var item in model.CartModel.CartItems)
			{
				basketItem = new BasketItem();
				basketItem.Id = item.ProductId.ToString();
				basketItem.Name = item.Name;
				basketItem.Category1 = "Phone";
				basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
				basketItem.Price = (item.Price * item.Quantity).ToString().Split(",")[0];

				basketItems.Add(basketItem);
			}

			request.BasketItems = basketItems;

			return Payment.Create(request, options);
		}
	}
}
