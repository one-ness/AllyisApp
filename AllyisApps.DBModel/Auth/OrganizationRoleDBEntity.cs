//------------------------------------------------------------------------------
// <copyright file="OrganizationRoleDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Represents the OrganizationRole table in the Database.
	/// </summary>
	public class OrganizationRoleDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the OrganizationRoleId.
		/// </summary>
		public int OrganizationRoleId { get; set; }

		/// <summary>
		/// Gets or sets the role name.
		/// </summary>
		public string Name { get; set; }
	}
}
