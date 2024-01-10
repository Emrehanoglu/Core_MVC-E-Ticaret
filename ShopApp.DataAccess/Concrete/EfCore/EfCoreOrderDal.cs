using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
	public class EfCoreOrderDal : EfCoreGenericRepository<Order, ShopContext>, IOrderDal
	{
		public List<Order> GetOrders(string userId)
		{
			using (var context = new ShopContext())
			{
				var orders = context.Orders.Include(i => i.OrderItems).ThenInclude(x => x.Product)
					.AsQueryable();

				if (!string.IsNullOrEmpty(userId))
				{
					orders = orders.Where(x => x.UserId == userId);
				}
				return orders.ToList();
			}
		}
	}
}
