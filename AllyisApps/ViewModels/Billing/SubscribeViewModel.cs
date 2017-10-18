//------------------------------------------------------------------------------
// <copyright file="SubscribeViewModel.cs" company="Allyis, Inc.">
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
	public class SubscribeViewModel : BaseViewModel
	{
		/// <summary>
		/// the sku the user is subscribing to
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// organization id
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// IcornUrl
		/// </summary>
		public string SkuIconUrl { get; set; }

		/// <summary>
		/// Gets or sets all information relating to this product.
		/// </summary>
		public string SkuName { get; set; }

		/// <summary>
		/// product name
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets a description of this product.
		/// </summary>
		public string SkuDescription { get; set; }

		/// <summary>
		/// Gets or sets the subscription name that is being unsubscribed from.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// indicates if user is only changing the sku in the same product
		/// </summary>
		public bool IsChanging { get; set; }
	}
}
