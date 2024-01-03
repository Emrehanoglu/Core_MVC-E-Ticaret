using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Models
{
	public class RegisterModel
	{
		[Required]
		public string FullName { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Compare("Password")]
		public string RePassword { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Mail { get; set; }
	}
}
