//------------------------------------------------------------------------------
// <copyright file="EditSubscriptionViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents an editable view of subcription.
	/// </summary>
	public class EditSubscriptionViewModel : BaseViewModel
	{
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
		/// Gets or sets the subscription id that is being unsubscribed from.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// organization id
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the subscription name that is being unsubscribed from.
		/// </summary>
		public string SubscriptionName { get; set; }
	}
}
