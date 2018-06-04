//------------------------------------------------------------------------------
// <copyright file="ProductIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// Products offered by us. Must match the Ids inserted in to db.
	/// </summary>
	public enum ProductIdEnum
	{
		/// <summary>
		/// Allyis Apps.
		/// </summary>
		AllyisApps = 100000,

		/// <summary>
		/// Time Tracker.
		/// </summary>
		TimeTracker = 200000,

		/// <summary>
		/// Expense Tracker.
		/// </summary>
		ExpenseTracker = 300000,

		/// <summary>
		/// Staffing Manager.
		/// </summary>
		StaffingManager = 400000,
	}
}