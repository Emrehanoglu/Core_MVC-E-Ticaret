using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUILayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Controllers
{
	public class AdminController : Controller
	{
		private IProductService _productService;
		private ICategoryService _categoryService;
		public AdminController(IProductService productService, ICategoryService categoryService)
		{
			_productService = productService;
			_categoryService = categoryService;
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
			return View(new ProductModel());
		}
		[HttpPost]
		public IActionResult CreateProduct(ProductModel model)
		{
			if (ModelState.IsValid)
			{
				var entity = new Product()
				{
					Name = model.Name,
					Price = model.Price,
					Description = model.Description,
					ImageUrl = model.ImageUrl
				};

				if (_productService.Create(entity))
				{
					return Redirect("Index");
				}
				ViewBag.ErrorMessage = _productService.ErrorMessage;
				return View(model);
			}

			return View(model);			
		}
		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if(id == null)
			{
				return NotFound();
			}
			var entity = _productService.GetByIdWithCategories((int)id);
			if (entity == null)
			{
				return NotFound();
			}
			var model = new ProductModel()
			{
				Id = entity.Id,
				Name = entity.Name,
				Price = entity.Price,
				Description = entity.Description,
				ImageUrl = entity.ImageUrl,
				SelectedCategories = entity.ProductCategories.Select(x => x.Category).ToList()
				//SelectedCategories ilgili ürünün olduğu kategoriler
			};
			ViewBag.Categories = _categoryService.GetAll();
			//Tüm kategoriler
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(ProductModel model, int[] categoryIds, IFormFile file)
		{
			if (ModelState.IsValid)
			{
				var entity = _productService.GetById(model.Id);
				if (entity == null)
				{
					return NotFound();
				}
				entity.Name = model.Name;
				entity.Description = model.Description;
				entity.Price = model.Price;

				//File Upload
				if(file != null)
				{
					entity.ImageUrl = file.FileName;
					var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
					using (var stream = new FileStream(path, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}
				}

				_productService.UpdateWithCategories(entity,categoryIds);
				return Redirect("/Admin/Products");
			}
			ViewBag.Categories = _categoryService.GetAll();
			//Tüm kategoriler
			return View(model);
		}
		[HttpPost]
		public IActionResult Delete(int productId)
		{
			var entity = _productService.GetById(productId);
			if(entity != null)
			{
				_productService.Delete(entity);
			}
			return Redirect("Index");
		}
		public IActionResult CategoryList()
		{
			return View(new CategoryListModel()
			{
				Categories = _categoryService.GetAll()
			});
		}
		[HttpGet]
		public IActionResult CreateCategory()
		{
			return View();
		}
		[HttpPost]
		public IActionResult CreateCategory(CategoryModel model)
		{
			var entity = new Category()
			{
				Name = model.Name
			};
			_categoryService.Create(entity);
			return Redirect("/Admin/CategoryList");
		}
		[HttpGet]
		public IActionResult EditCategory(int id)
		{
			var entity = _categoryService.GetByIdWithProducts(id);
			if(entity == null)
			{
				return NotFound();
			}
			return View(new CategoryModel() 
			{
				Id = entity.Id,
				Name = entity.Name,
				Products = entity.ProductCategories.Select(p => p.Product).ToList()
			});
		}
		[HttpPost]
		public IActionResult EditCategory(CategoryModel model)
		{
			var entity = _categoryService.GetByIdWithProducts(model.Id);
			if (entity == null)
			{
				return NotFound();
			}
			entity.Name = model.Name;
			_categoryService.Update(entity);
			return Redirect("/Admin/CategoryList");
		}
		[HttpPost]
		public IActionResult DeleteCategory(int categoryId)
		{
			var entity = _categoryService.GetById(categoryId);
			if (entity == null)
			{
				return NotFound();
			}
			_categoryService.Delete(entity);
			return Redirect("/Admin/CategoryList");
		}
		[HttpPost]
		public IActionResult DeleteFromCategory(int categoryId,int productId)
		{
			_categoryService.DeleteFromCategory(categoryId,productId);
			return Redirect("/Admin/EditCategory/"+categoryId);
		}
	}
}
