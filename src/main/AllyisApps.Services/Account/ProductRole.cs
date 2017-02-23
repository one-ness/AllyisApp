//------------------------------------------------------------------------------
// <copyright file="ProductRole.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// Subscription role.
	/// </summary>
	public enum ProductRole : int
	{
		/// <summary>
		/// Not in product.
		/// </summary>
		NotInProduct = 0,

		/// <summary>
		/// TimeTracker User.
		/// </summary>
		TimeTrackerUser = ProductRoleIdEnum.TimetrackerUser,

		/// <summary>
		/// TimeTracker Manager.
		/// </summary>
		TimeTrackerManager = ProductRoleIdEnum.TimetrackerManager,

		/// <summary>
		/// TimerTracker User.
		/// </summary>
		ConsultingUser = ProductRoleIdEnum.ConsultingUser,

		/// <summary>
		/// Consulting Manager.
		/// </summary>
		ConsultingManager = ProductRoleIdEnum.ConsultingManager,

		/// <summary>
		/// Consuilting Admin.
		/// </summary>
		ConsultingAdmin = ProductRoleIdEnum.ConsultingAdmin,
	}
}
