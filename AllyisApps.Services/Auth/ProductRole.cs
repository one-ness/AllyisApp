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
		/// gets or sets the short name.
		/// </summary>
		public string ProductRoleShortName { get; set; }

		/// <summary>
		/// gets or sets the full name
		/// </summary>
		public string ProductRoleFullName { get; set; }

		/// <summary>
		/// Gets or sets ProductRoleId.
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Product Id.
		/// </summary>
		public ProductIdEnum ProductId { get; set; }

		/// <summary>
		/// the organization or subscription this role belongs to
		/// </summary>
		public int OrgOrSubId { get; set; }

		/// <summary>
		/// Gets or sets the builtin product role id
		/// </summary>
		public BuiltinProductRoleIdEnum BuiltinProductRoleId { get; set; }
	}
}
