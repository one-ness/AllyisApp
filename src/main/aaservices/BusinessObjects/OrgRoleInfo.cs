//------------------------------------------------------------------------------
// <copyright file="OrgRoleInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// Represents an organization role.
	/// </summary>
	public class OrgRoleInfo
	{
		/// <summary>
		/// Gets or sets the Organization role Id.
		/// </summary>
		public int OrgRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Organization role name.
		/// </summary>
		public string OrgRoleName { get; set; }
	}
}
