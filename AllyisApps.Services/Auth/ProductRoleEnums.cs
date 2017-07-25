﻿//------------------------------------------------------------------------------
// <copyright file="ProductRoleIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// Product role aka Subscription role.
	/// </summary>
	public enum TimeTrackerRole : int
	{
		/// <summary>
		/// Not in TimeTracker product.
		/// </summary>
		NotInProduct = 0,

		/// <summary>
		/// TimeTracker User.
		/// </summary>
		User = 1,

		/// <summary>
		/// TimeTracker Manager.
		/// </summary>
		Manager = 2
	}
}
