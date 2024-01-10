using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
	public class EfCoreCartDal : EfCoreGenericRepository<Cart, ShopContext>, ICartDal
	{
		public override void Update(Cart entity)
		{
			using (var context = new ShopContext())
			{
				context.Carts.Update(entity);
				context.SaveChanges();
			}
		}
		public Cart GetCartByUserId(string userId)
		{
			using (var context = new ShopContext())
			{
				return context.Carts.Include(i => i.CartItems)
					.ThenInclude(x => x.Product)
					.FirstOrDefault(x=>x.UserId == userId);
			}
		}

		public void DeleteFromCart(int cartId, int productId)
		{
			using (var context = new ShopContext())
			{
				var cmd = @"delete from CartItems where CartId=@p0 and ProductId=@p1";
				context.Database.ExecuteSqlCommand(cmd,cartId,productId);
				context.SaveChanges();
			}
		}

		public void ClearCart(string cartId)
		{
			using (var context = new ShopContext())
			{
				var cmd = @"delete from CartItems where CartId=@p0";
				context.Database.ExecuteSqlCommand(cmd, cartId);
				context.SaveChanges();
			}
		}
	}
}
