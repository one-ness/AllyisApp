//------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// Represents a product.
	/// </summary>
	public class Product
	{
		/// <summary>
		/// Gets or sets the product Id.
		/// </summary>
		public ProductIdEnum ProductId { get; set; }

		/// <summary>
		/// Gets or sets the product Name.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the product description.
		/// </summary>
		public string ProductDescription { get; set; }

		/// <summary>
		/// Gets or sets the area url.
		/// </summary>
		public string AreaUrl { get; set; }

		/// <summary>
		/// Gets or sets the product status
		/// </summary>
		[CLSCompliant(false)]
		public ProductStatusEnum ProductStatus { get; set; }
	}
}