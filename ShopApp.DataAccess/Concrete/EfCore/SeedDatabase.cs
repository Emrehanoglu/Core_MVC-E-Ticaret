using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
	public static class SeedDatabase
	{
		public static void Seed()
		{
			var context = new ShopContext();
			if (context.Database.GetPendingMigrations().Count() == 0)
				//bekleyen migration var mı yok mu kontrolü
			{
				if(context.Categories.Count() == 0)
				{
					context.Categories.AddRange(Categories);
				}
				if (context.Products.Count() == 0)
				{
					context.Products.AddRange(Products);
					context.AddRange(ProductCategory);
				}
				context.SaveChanges();
			}
		}
		private static Category[] Categories =
		{
			new Category() {Name="Telefon"},
			new Category() {Name="Bilgisayar"},
			new Category() {Name="Elektronik"}
		};
		private static Product[] Products =
		{
			new Product() {Name="IPhone 11",Price=11000,ImageUrl="1.jpg",Description="<p>Güzel Telefon</p>"},
			new Product() {Name="IPhone 12",Price=12000,ImageUrl="2.jpg",Description="<p>Güzel Telefon</p>"},
			new Product() {Name="IPhone 13",Price=13000,ImageUrl="3.jpg",Description="<p>Güzel Telefon</p>"},
			new Product() {Name="IPhone 14",Price=14000,ImageUrl="4.jpg",Description="<p>Güzel Telefon</p>"},
			new Product() {Name="IPhone 15",Price=15000,ImageUrl="5.jpg",Description="<p>Güzel Telefon</p>"}
		};
		private static ProductCategory[] ProductCategory =
		{
			new ProductCategory(){ Product=Products[0], Category=Categories[0] },
			new ProductCategory(){ Product=Products[0], Category=Categories[2] },
			new ProductCategory(){ Product=Products[1], Category=Categories[0] },
			new ProductCategory(){ Product=Products[1], Category=Categories[1] },
			new ProductCategory(){ Product=Products[2], Category=Categories[0] },
			new ProductCategory(){ Product=Products[2], Category=Categories[2] },
			new ProductCategory(){ Product=Products[3], Category=Categories[1] },
		};
	}
}
