using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShopApp.DataAccess.Concrete.MySql
{
	public class MySqlProductDal : IProductDal
	{
		public void Create(Product product)
		{
			throw new NotImplementedException();
		}

		public void Delete(Product product)
		{
			throw new NotImplementedException();
		}

		public Product GetById(int id)
		{
			throw new NotImplementedException();
		}

		public Product GetOne(Expression<Func<Product, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public void Update(Product product)
		{
			throw new NotImplementedException();
		}

		IQueryable<Product> IProductDal.GetAll(Expression<Func<Product, bool>> filter)
		{
			throw new NotImplementedException();
		}
	}
}
