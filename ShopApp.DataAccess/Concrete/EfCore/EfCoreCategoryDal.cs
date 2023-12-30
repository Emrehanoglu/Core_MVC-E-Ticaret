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
	public class EfCoreCategoryDal : EfCoreGenericRepository<Category, ShopContext>, ICategoryDal
	{
		public void DeleteFromCategory(int categoryId, int productId)
		{
			using (var context = new ShopContext())
			{
				var cmd = @"delete from ProductCategory where ProductId=@p0 and CategoryId=@p1";
				context.Database.ExecuteSqlCommand(cmd,productId,categoryId);
			}
		}

		public Category GetByIdWithProducts(int id)
		{
			using (var context = new ShopContext())
			{
				return context.Categories.Where(x => x.Id == id).Include(i => i.ProductCategories)
					.ThenInclude(s => s.Product).FirstOrDefault();
				//Bir kategoriye ait bütün ürünleri alıyorum.
			};
		}
	}
}
