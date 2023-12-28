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
	public class AdminController : Controller
	{
		private IProductService _productService;
		public AdminController(IProductService productService)
		{
			_productService = productService;
		}

		public IActionResult Index()
		{
			return View(new ProductListModel() 
			{
				Products = _productService.GetAll()
			});
		}
		[HttpGet]
		public IActionResult CreateProduct()
		{
			return View();
		}
		[HttpPost]
		public IActionResult CreateProduct(ProductModel model)
		{
			var entity = new Product()
			{
				Name = model.Name,
				Price = model.Price,
				Description = model.Description,
				ImageUrl = model.ImageUrl
			};
			_productService.Create(entity);
			return Redirect("Index");
		}
	}
}
