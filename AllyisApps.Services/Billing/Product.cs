//------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// Represents a product.
	/// </summary>
	public class Product
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
