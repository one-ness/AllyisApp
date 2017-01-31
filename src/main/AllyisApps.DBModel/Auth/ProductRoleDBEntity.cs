//------------------------------------------------------------------------------
// <copyright file="ProductRoleDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// The Product Role Table Poco.
	/// </summary>
	public class ProductRoleDBEntity
	{
		/// <summary>
		/// Gets or sets the ProductRoleId.
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the ProductId.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the PermissionAdmin bit.
		/// </summary>
		public bool PermissionAdmin { get; set; }

		/// <summary>
		/// Gets or sets the CreatedUTC.
		/// </summary>
		public DateTime CreatedUTC { get; set; }

		/// <summary>
		/// Gets or sets the ModifiedUTC.
		/// </summary>
		public DateTime ModifiedUTC { get; set; }
	}
}