//------------------------------------------------------------------------------
// <copyright file="OrganizationRole.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Auth;

namespace AllyisApps.Services.Account
{
	/// <summary>
	/// Role of the user in an organization.
	/// </summary>
	public enum OrganizationRole : int
	{
		/// <summary>
		/// Organization member.
		/// </summary>
		Member = OrganizationRoleIdEnum.Member,

		/// <summary>
		/// Organization owner.
		/// </summary>
		Owner = OrganizationRoleIdEnum.Owner,
	}
}
