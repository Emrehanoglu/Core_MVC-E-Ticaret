using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUILayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Controllers
{
	public class ShopController : Controller
	{
		private IProductService _productService;
		public ShopController(IProductService productService)
		{
			_productService = productService;
		}
		// /products/telefon?page=3 --> 3.sayfa
		public IActionResult List(string category, int page=1)
		{
			const int pageSize = 3; //her sayfada gosterilecek ürün sayısı
			return View(new ProductListModel()
			{
				PageInfo = new PageInfo()
				{
					TotalItems = _productService.GetCountByCategory(category),
					CurrentPage = page,
					CurrentCategory = category,
					ItemsPerPage = pageSize
				},
				Products = _productService.GetProductsByCategory(category, page, pageSize)
			});
		}
		public IActionResult Details(int? id)
		{
			if(id == null)
			{
				return NotFound(); //Kullanıcıyı 404 sayfasına yonlendirdim
			}
			Product product = _productService.GetProductDetails((int)id);
			if(product == null)
			{
				return NotFound(); //Kullanıcıyı 404 sayfasına yonlendirdim
			}
			return View(new ProductDetailsModel()
			{
				Product = product,
				Categories = product.ProductCategories.Select(i => i.Category).ToList()
			}); ;
		}
	}
}
