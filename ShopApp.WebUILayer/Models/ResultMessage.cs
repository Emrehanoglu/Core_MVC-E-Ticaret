﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUILayer.Models
{
	public class ResultMessage
	{
		public string Title { get; set; }
		public string Message { get; set; }
		public string Css { get; set; } //success, warning, danger
	}
}
