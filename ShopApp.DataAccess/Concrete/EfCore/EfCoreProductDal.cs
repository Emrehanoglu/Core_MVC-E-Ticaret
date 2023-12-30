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
		public Product GetByIdWithCategories(int id)
		{
			using (var context = new ShopContext())
			{
				return context.Products.Where(x => x.Id == id).Include(s => s.ProductCategories)
					.ThenInclude(s => s.Category).FirstOrDefault();
			}
		}
		public int GetCountByCategory(string category)
		{
			using (var context = new ShopContext())
			{
				var products = context.Products.AsQueryable();
				if (!string.IsNullOrEmpty(category))
				{
					products = products.Include(i => i.ProductCategories)
						.ThenInclude(i => i.Category)
						.Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
				}
				return products.Count();
			}
		}

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

		public List<Product> GetProductsByCategory(string category, int page, int pageSize)
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
				return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
				//page:1,pageSize=3 olsun, 1-1=0,0*3=0,Skip ile 0 tane ürünü geç,Take ile ilk 3 tane ürünü al
				//page:2,pageSize=3 olsun, 2-1=1,1*3=3,Skip ile 3 tane ürünü geç,Take ile ikinci 3 tane ürünü al
				//page:3,pageSize=3 olsun, 3-1=2,2*3=6,Skip ile 6 tane ürünü geç,Take ile üçüncü 3 tane ürünü al
			}
		}
	}
}
