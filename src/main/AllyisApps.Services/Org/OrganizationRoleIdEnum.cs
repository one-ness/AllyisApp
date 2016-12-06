//------------------------------------------------------------------------------
// <copyright file="OrganizationRoleIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

#pragma warning disable 1591

namespace AllyisApps.Services
{
	/// <summary>
	/// Organization roles of a user.
	/// - Must be exactly same as stored in the OrganizationRole table in DB.
	/// </summary>
	public enum OrganizationRoleIdEnum : int
	{
		/// <summary>
		/// Belongs to the organization.
		/// </summary>
		Member = 1,

		/// <summary>
		/// Owns the organization.
		/// </summary>
		Owner = 2,
	}
}