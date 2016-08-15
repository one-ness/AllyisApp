//------------------------------------------------------------------------------
// <copyright file="ProductRoleInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// Represents a product role.
	/// </summary>
	public class ProductRoleInfo
	{
		/// <summary>
		/// Gets or sets the Product role Id.
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Product Id.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets the Product role name.
		/// </summary>
		public string ProductRoleName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this product role has Admin permission.
		/// </summary>
		public bool PermissionAdmin { get; set; }

		/// <summary>
		/// Gets or sets The time created.
		/// </summary>
		public DateTime CreatedUTC { get; set; }

		/// <summary>
		/// Gets or sets The last time modified.
		/// </summary>
		public DateTime ModifiedUTC { get; set; }
	}
}
