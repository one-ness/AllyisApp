//------------------------------------------------------------------------------
// <copyright file="OrgRoleDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Represents the OrganizationRole table in the Database.
	/// </summary>
	public class OrgRoleDBEntity
	{
		/// <summary>
		/// Gets or sets the OrgRoleID.
		/// </summary>
		public int OrgRoleId { get; set; }

		/// <summary>
		/// Gets or sets the role name.
		/// </summary>
		public string Name { get; set; }
	}
}
