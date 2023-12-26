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
		public IActionResult List()
		{
			return View(new ProductListModel()
			{
				Products = _productService.GetAll()
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
