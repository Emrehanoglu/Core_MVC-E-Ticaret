﻿using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShopApp.DataAccess.Abstract
{
	public interface IProductDal : IRepository<Product>
	{
		List<Product> GetPopularProducts();
		List<Product> GetProductsByCategory(string category, int page, int pageSize);
		Product GetProductDetails(int id);
		Product GetByIdWithCategories(int id);
		int GetCountByCategory(string category);
		void UpdateWithCategories(Product entity, int[] categoryIds);
	}
}
