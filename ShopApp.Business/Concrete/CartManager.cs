using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
	public class CartManager : ICartService
	{
		private ICartDal _cartDal;
		public CartManager(ICartDal cartDal)
		{
			_cartDal = cartDal;
		}

		public void AddToCart(string userId, int productId, int quantity)
		{
			//kullanıcıya ait bir cart var mı
			var cart = _cartDal.GetCartByUserId(userId);
			if (cart != null)
			{
				//kullanıcı daha once ayunı ürünü eklemişmi
				var index = cart.CartItems.FindIndex(i => i.ProductId == productId);

				if (index < 0) //bu kayıt sepette yok ise
				{
					cart.CartItems.Add(new CartItem
					{
						ProductId = productId,
						Quantity = quantity,
						CartId = cart.Id
					});
				}
				else //bu üründen sepette var, quantity 'sini quantity kadar arttıralım
				{
					cart.CartItems[index].Quantity += quantity;
				}

				_cartDal.Update(cart);
			}
		}

		public Cart GetCartByUserId(string userId)
		{
			return _cartDal.GetCartByUserId(userId);
		}

		public void InitializeCart(string userId)
		{
			_cartDal.Create(new Cart()
			{
				UserId = userId
			});
		}
	}
}
