//using ShopApp.DataAccess.Abstract;
//using ShopApp.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Text;

//namespace ShopApp.DataAccess.Concrete.Memory
//{
//	public class MemoryProductDal : IProductDal
//	{
//		public void Create(Product entity)
//		{
//			throw new NotImplementedException();
//		}

//		public void Delete(Product entity)
//		{
//			throw new NotImplementedException();
//		}

//		public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
//		{
//			var products = new List<Product>()
//			{
//				new Product() {Id=1,Name="IPhone 11",ImageUrl="1.jpg",Price=11000},
//				new Product() {Id=2,Name="IPhone 12",ImageUrl="2.jpg",Price=12000},
//				new Product() {Id=3,Name="IPhone 13",ImageUrl="3.jpg",Price=13000}
//			};
//			return products;
//		}

//		public Product GetById(int id)
//		{
//			throw new NotImplementedException();
//		}

//		public Product GetOne(Expression<Func<Product, bool>> filter)
//		{
//			throw new NotImplementedException();
//		}

//		public List<Product> GetPopularProducts()
//		{
//			throw new NotImplementedException();
//		}

//		public Product GetProductDetails(int id)
//		{
//			throw new NotImplementedException();
//		}

//		public void Update(Product entity)
//		{
//			throw new NotImplementedException();
//		}
//	}
//}
