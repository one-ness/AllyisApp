//------------------------------------------------------------------------------
// <copyright file="OrganizationRole.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// Role of the user in an organization.
	/// </summary>
	public enum OrganizationRole : int
	{
		/// <summary>
		/// Organization member.
		/// </summary>
		Member = 1,

		/// <summary>
		/// Organization owner.
		/// </summary>
		Owner = 2
	}
}