using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Controllers
{
	public class HomeController : Controller
	{
		private IProductService _productService;

		public HomeController(IProductService productService)
		{
			_productService = productService;
		}

		public IActionResult Index()
		{
			return View(_productService.GetAll());
		}
	}
}
