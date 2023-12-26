using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
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
	}
}
