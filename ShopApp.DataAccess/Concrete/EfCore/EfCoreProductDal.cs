using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
	public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
	{
		public List<Product> GetPopularProducts()
		{
			using (var context = new ShopContext())
			{
				return context.Set<Product>().Where(x => x.Price > 13000).ToList();
			}
		}

		public Product GetProductDetails(int id)
		{
			using (var context = new ShopContext())
			{
				return context.Products.Where(x => x.Id == id).Include(i => i.ProductCategories)
					.ThenInclude(i => i.Category).FirstOrDefault();
			}
		}
	}
}
