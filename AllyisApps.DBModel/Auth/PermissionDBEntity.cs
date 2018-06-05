//------------------------------------------------------------------------------
// <copyright file="PermissionDBEntity.cs" company="Allyis, Inc.">
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
	/// permission db entity
	/// </summary>
	public class PermissionDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the product role for which this permission is set
		/// </summary>
		public int ProductRoleId { get; set; }

		/// <summary>
		/// Gets or sets the action group for which this permission applies
		/// </summary>
		public int ActionGroupId { get; set; }

		/// <summary>
		/// Gets or sets the action for which this permission applies
		/// </summary>
		public int UserActionId { get; set; }

		/// <summary>
		/// Gets or sets if this action on this entity is allowed or not
		/// </summary>
		public bool IsDenied { get; set; }
	}
}
