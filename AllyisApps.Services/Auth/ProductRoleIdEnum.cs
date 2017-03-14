//------------------------------------------------------------------------------
// <copyright file="ProductRoleIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// Subscription role.
	/// </summary>
	public enum ProductRoleIdEnum : int
	{
		/// <summary>
		/// Not in product.
		/// </summary>
		NotInProduct = 0,

		/// <summary>
		/// TimeTracker User.
		/// </summary>
		TimeTrackerUser = 1,

		/// <summary>
		/// TimeTracker Manager.
		/// </summary>
		TimeTrackerManager = 2
	}
}
