using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShopApp.Business.Concrete
{
	public class ProductManager : IProductService
	{
		private IProductDal _productDal;
		public ProductManager(IProductDal productDal)
		{
			_productDal = productDal;
		}

		public bool Create(Product entity)
		{
			if (Validate(entity))
			{
				_productDal.Create(entity);
				return true;
			}
			return false;
		}

		public void Delete(Product entity)
		{
			_productDal.Delete(entity);
		}

		public List<Product> GetAll()
		{
			return _productDal.GetAll();
		}

		public Product GetById(int id)
		{
			return _productDal.GetById(id);
		}

		public Product GetByIdWithCategories(int id)
		{
			return _productDal.GetByIdWithCategories(id);
		}

		public int GetCountByCategory(string category)
		{
			return _productDal.GetCountByCategory(category);
		}

		public List<Product> GetPopularProducts()
		{
			return _productDal.GetPopularProducts();
		}

		public Product GetProductDetails(int id)
		{
			return _productDal.GetProductDetails(id);
		}

		public List<Product> GetProductsByCategory(string category, int page, int pageSize)
		{
			return _productDal.GetProductsByCategory(category, page,pageSize);
		}

		public void Update(Product entity)
		{
			_productDal.Update(entity);
		}

		public void UpdateWithCategories(Product entity, int[] categoryIds)
		{
			_productDal.UpdateWithCategories(entity, categoryIds);
		}

		public bool Validate(Product entity)
		{
			var isValid = true;
			if (string.IsNullOrEmpty(entity.Name))
			{
				ErrorMessage += "Ürün ismi girmelisiniz";
				isValid = false;
			}
			return isValid;
		}
		public string ErrorMessage { get; set; }
	}
}
