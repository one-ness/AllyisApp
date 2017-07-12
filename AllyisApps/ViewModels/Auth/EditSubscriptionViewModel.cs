//------------------------------------------------------------------------------
// <copyright file="EditSubscriptionViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets the current product's Id.
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// Gets or sets all information relating to this product.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a description of this product.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a description of this product.
		/// </summary>
		public string isActive { get; set; }

		/// <summary>
		/// the subscritpion that is being unsubscribed from
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the current product's Id.
		/// </summary>
		public int UserLimit { get; set; }
	}
}