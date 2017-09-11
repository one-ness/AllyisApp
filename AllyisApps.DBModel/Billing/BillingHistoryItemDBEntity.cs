//------------------------------------------------------------------------------
// <copyright file="BillingHistoryItemDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Billing
{
	/// <summary>
	/// Represents an item in the billing history of an organization.
	/// </summary>
	public class BillingHistoryItemDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the user Id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the user name.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the sku Id.
		/// </summary>
		public int SkuId { get; set; }

		/// <summary>
		/// Gets or sets sku name.
		/// </summary>
		public string SkuName { get; set; }

		/// <summary>
		/// Gets or sets product Id.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets the product name.
		/// </summary>
		public string ProductName { get; set; }
	}
}