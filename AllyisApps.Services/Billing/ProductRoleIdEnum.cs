//------------------------------------------------------------------------------
// <copyright file="ProductIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// Products offered by us. Must match the Ids inserted in to db.
	/// </summary>
	public enum ProductRoleIdEnum : int
	{
		/// <summary>
		/// Organization member.
		/// </summary>
		User = 1,

		/// <summary>
		/// Organization owner.
		/// </summary>
		Manager = 2
	}
}
