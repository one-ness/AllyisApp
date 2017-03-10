//------------------------------------------------------------------------------
// <copyright file="BillingHistoryItemInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// Represents an item in the billing history of an organization.
	/// </summary>
	public class BillingHistoryItemInfo
	{
		/// <summary>
		/// Gets or sets the organization ID.
		/// </summary>
		public int OrganizationID { get; set; }

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the user ID.
		/// </summary>
		public int UserID { get; set; }

		/// <summary>
		/// Gets or sets the user name.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the sku ID.
		/// </summary>
		public int SkuID { get; set; }

		/// <summary>
		/// Gets or sets sku name.
		/// </summary>
		public string SkuName { get; set; }

		/// <summary>
		/// Gets or sets product ID.
		/// </summary>
		public int ProductID { get; set; }

		/// <summary>
		/// Gets or sets the product name.
		/// </summary>
		public string ProductName { get; set; }
	}
}
