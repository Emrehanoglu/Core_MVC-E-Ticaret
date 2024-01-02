using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Models
{
	public class ProductModel
	{
		public int Id { get; set; }
		//[Required]
		//[StringLength(60,MinimumLength =10,ErrorMessage ="Ürün ismi en az 10 karakter olmalıdır.")]
		public string Name { get; set; }
		[Required]
		public string ImageUrl { get; set; }
		[Required]
		[StringLength(100, MinimumLength = 20, ErrorMessage = "Ürün açıklaması en az 20 karakter olmalıdır.")]
		public string Description { get; set; }
		[Required(ErrorMessage ="Fiyat belirtiniz")]
		[Range(1,10000)]
		public decimal? Price { get; set; }
		public List<Category> SelectedCategories { get; set; }
	}
}
