//------------------------------------------------------------------------------
// <copyright file="ProductRoleDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// product role db entity
	/// </summary>
	public class ProductRoleDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the product role id
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the product id
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets the short name
		/// </summary>
		public string ProductRoleShortName { get; set; }

		/// <summary>
		/// Gets or sets the full name
		/// </summary>
		public string ProductRoleFullName { get; set; }

		/// <summary>
		/// Gets or sets the organization or the subscription id, this product role pertains to
		/// </summary>
		public int OrgOrSubId { get; set; }

		/// <summary>
		/// Gets or sets the built-in role id
		/// </summary>
		public bool BuiltInProductRoleId { get; set; }
	}
}
