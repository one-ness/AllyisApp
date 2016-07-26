//------------------------------------------------------------------------------
// <copyright file="ProductInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.Crm
{
	/// <summary>
	/// Represents a product.
	/// </summary>
	public class ProductInfo
	{
		/// <summary>
		/// Gets or sets the product Id.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets the product Name.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the product description.
		/// </summary>
		public string ProductDescription { get; set; }
	}
}
