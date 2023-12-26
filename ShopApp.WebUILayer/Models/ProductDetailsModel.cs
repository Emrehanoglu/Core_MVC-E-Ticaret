using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Models
{
	public class ProductDetailsModel
	{
		public Product Product { get; set; }
		public List<Category> Categories { get; set; }
	}
}
