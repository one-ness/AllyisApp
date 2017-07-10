//------------------------------------------------------------------------------
// <copyright file="ProductIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// Products offered by us. Must match the IDs inserted in to db.
	/// </summary>
	public enum ProductIdEnum : short
	{
		/// <summary>
		/// ID for no product.
		/// </summary>
		None = 0,

		/// <summary>
		/// Time Tracker.
		/// </summary>
		TimeTracker = 1,

		/// <summary>
		/// Expense Tracker
		/// </summary>
		ExpenseTracker = 2,
	}
}
