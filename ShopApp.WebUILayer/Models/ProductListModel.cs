using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Models
{
	public class PageInfo
	{
		public int TotalItems { get; set; }			//toplam eleman sayısı
		public int ItemsPerPage { get; set; }		//her sayfadaki eleman sayısı
		public int CurrentPage { get; set; }		//o an ki aktif sayfa numarası
		public string CurrentCategory { get; set; } //o an ki aktif kategori bilgisi
		public int TotalPages() 
		{
			return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); // 10/3 = 3,3 --> 4 sayfa olmalı
		}
	}
	public class ProductListModel
	{
		public PageInfo PageInfo { get; set; }
		public List<Product> Products { get; set; }
	}
}
