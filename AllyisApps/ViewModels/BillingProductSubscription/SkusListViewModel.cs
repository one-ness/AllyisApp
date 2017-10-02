//------------------------------------------------------------------------------
// <copyright file="SkusListViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;

using AllyisApps.Services.Billing;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the Skus List view.
	/// </summary>
	public class SkusListViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the collection of all products offered to user.
		/// </summary>
		public IEnumerable<ProductViewModel> ProductsList { get; set; }

		/// <summary>
		/// Gets or sets the current Organization's Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the current Organization's active subscriptions.
		/// </summary>
		public IEnumerable<SubscriptionDisplayViewModel> CurrentSubscriptions { get; set; }

		/// <summary>
		/// Product View Model for Subscripotion edit
		/// </summary>
		public class ProductViewModel
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
			/// Gets or sets the list of Skus under this product.
			/// </summary>
			public List<SkuInfoViewModel> ProductSkus { get; set; }

			/// <summary>
			/// Gets or sets the area url.
			/// </summary>
			public string AreaUrl { get; set; }

			/// <summary>
			/// SKU Info
			/// </summary>
			public class SkuInfoViewModel
			{
				/// <summary>
				/// Gets or sets the Id of the entry in the SKU table.
				/// </summary>
				public SkuIdEnum SkuId { get; set; }

				/// <summary>
				/// Gets or sets the SkuId of the upgrade or downgrade version of the Product.
				/// </summary>
				public SkuIdEnum SkuIdNext { get; set; }

				/// <summary>
				/// Gets or sets the products id number.
				/// </summary>
				public ProductIdEnum ProductId { get; set; }

				/// <summary>
				/// Gets or sets name of the sku.
				/// </summary>
				public string SkuName { get; set; }

				/// <summary>
				/// Gets or sets the description of the sku.
				/// </summary>
				public string Description { get; set; }

				/// <summary>
				/// Icon Url for Sku
				/// </summary>
				public string IconUrl { get; set; }

				/// <summary>
				/// Gets or sets the price.
				/// </summary>
				public decimal Price { get; set; }
			}
		}
	}
}