//------------------------------------------------------------------------------
// <copyright file="EditSubscriptionViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Services.Billing;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents an editable view of subcription.
	/// </summary>
	public class EditSubscriptionViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the current product's Id.
		/// </summary>
		public int BillingFrequency { get; set; }

		/// <summary>
		/// Gets or sets the current product's Id.
		/// </summary>
		public int CostPerBlock { get; set; }

		/// <summary>
		/// Gets or sets the current product's Id.
		/// </summary>
		public ProductIdEnum ProductId { get; set; }

		/// <summary>
		/// Gets or sets the current product's Name.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the current product's Id.
		/// </summary>
		public SkuIdEnum SkuId { get; set; }

		/// <summary>
		/// Gets or sets the SkuId of the upgrade or downgrade version of the Product.
		/// </summary>
		public SkuIdEnum SkuIdNext { get; set; }

		/// <summary>
		/// IcornUrl
		/// </summary>
		public string SkuIconUrl { get; set; }

		/// <summary>
		/// Gets or sets all information relating to this product.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the SkuId of the upgrade or downgrade version of the Product.
		/// </summary>
		public string NextName { get; set; }

		/// <summary>
		/// Gets or sets a description of this product.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a description of this product.
		/// </summary>
		public string IsActive { get; set; }

		/// <summary>
		/// Gets or sets the subscritpion that is being unsubscribed from.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the subscription id that is being unsubscribed from.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the subscription name that is being unsubscribed from.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the current product's Id.
		/// </summary>
		public int UserLimit { get; set; }

		/// <summary>
		/// Gets or sets the type of edit being done (upgrade, downgrade, unsubscribe).
		/// </summary>
		public SkuIdEnum? SelectedNewSkuEnum { get; set; }

		/// <summary>
		/// Gets or sets otherSkus for the edit being made.
		/// </summary>
		public IEnumerable<SkuIdEnum> OtherSkus { get; set; }
	}
}