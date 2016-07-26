//------------------------------------------------------------------------------
// <copyright file="LastUsedDBEntity.cs" company="Allyis, Inc.">
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
	/// LastUsedDBEntity obj for getting the last subscription used by user.  
	/// </summary>
	public class LastUsedDBEntity : BasePoco
	{
		/// <summary>
		/// Gets or sets Organization.
		/// </summary>
		public string Organization { get; set; }

		/// <summary>
		/// Gets or sets OrganizationId.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets Product.
		/// </summary>
		public string Product { get; set; }

		/// <summary>
		/// Gets or sets ProductId.
		/// </summary>
		public int ProductId { get; set; }
	}
}
