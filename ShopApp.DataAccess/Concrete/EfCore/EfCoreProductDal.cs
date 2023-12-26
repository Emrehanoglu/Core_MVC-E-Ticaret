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

		public List<Product> GetProductsByCategory(string category)
		{
			using (var context = new ShopContext())
			{
				var products = context.Products.AsQueryable();
				//Queryable yaptım. henüz ToList() demiyorum.
				//Sorgu hazır şekilde elimde şuan. product.ToList() diyerek kullanabilirim
				//ya da sorguya yeni koşullar ekleyerek kullanabilirim.

				//kategori bilgisi null ise tüm products listesi dönecek.
				if (!string.IsNullOrEmpty(category))
				{
					//Gönderdiğim kategori bilgisine göre ürünler filtrelendi.
					products = products.Include(i => i.ProductCategories)
						.ThenInclude(i => i.Category)
						.Where(i=>i.ProductCategories.Any(a=>a.Category.Name.ToLower() == category.ToLower()));
					//ProductCategories.Any() ile true ya da false değer almış oldum.
				}
				return products.ToList();
			}
		}
	}
}
