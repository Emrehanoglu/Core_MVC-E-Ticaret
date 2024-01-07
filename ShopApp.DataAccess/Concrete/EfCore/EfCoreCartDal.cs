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
	}
}
