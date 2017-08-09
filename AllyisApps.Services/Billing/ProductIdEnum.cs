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
	public enum ProductIdEnum : int
	{
		/// <summary>
		/// Id for no product.
		/// </summary>
		None = 0,

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
