//------------------------------------------------------------------------------
// <copyright file="BillingHistoryItemViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// The billing history model.
	/// </summary>
	public class BillingHistoryItemViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the date/time of this item.
		/// </summary>
		public long Date { get; set; }

		/// <summary>
		/// Gets or sets the stipe ID of this item if it is an invoice or charge.
		/// </summary>
		public string ID { get; set; }

		/// <summary>
		/// Gets or sets a description of this item.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the name of the product and/or sku.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the username of the user who performed this action, if available.
		/// </summary>
		public string Username { get; set; }
	}
}
