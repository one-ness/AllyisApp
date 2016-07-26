//------------------------------------------------------------------------------
// <copyright file="ProductRoleIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

#pragma warning disable 1591

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Product roles of a user.
	/// - Must be exactly same as stored in the ProductRole table in DB.
	/// </summary>
	public enum ProductRoleIdEnum : int
	{
		/// <summary>
		/// Role: user.
		/// </summary>
		TimetrackerUser = 1,

		/// <summary>
		/// Role: Manager.
		/// </summary>
		TimetrackerManager = 2,

		/// <summary>
		/// Role: Admin.
		/// </summary>
		TimetrackerAdmin = 3,

		/// <summary>
		/// Role: User.
		/// </summary>
		ConsultingUser = 4,

		/// <summary>
		/// Role: Manager.
		/// </summary>
		ConsultingManager = 5,

		/// <summary>
		/// Role: Admin.
		/// </summary>
		ConsultingAdmin = 6,
	}
}
