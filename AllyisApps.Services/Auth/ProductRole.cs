//------------------------------------------------------------------------------
// <copyright file="ProductRole.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.Billing;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// product role obj.
	/// </summary>
	public class ProductRole
	{
		/// <summary>
		/// indicates that user is not assigned this product yet.
		/// </summary>
		public const int NotInProduct = 0;

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string ProductRoleName { get; set; }

		/// <summary>
		/// Gets or sets ProductRoleId.
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Product Id.
		/// </summary>
		public ProductIdEnum ProductId { get; set; }

		/// <summary>
		/// the organization this role belongs to
		/// </summary>
		public int OrganizationId { get; set; }
	}
}
