//------------------------------------------------------------------------------
// <copyright file="ProductRoleEnum.cs" company="Allyis, Inc.">
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
		/// TimeTracker Unavailable.
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

    public enum ExpenseTrackerRole : int
    {
        /// <summary>
        /// Expense Tracker Unavailable.
        /// </summary>
        NotInProduct = 0,

        /// <summary>
        /// Expense Tracker User.
        /// </summary>
        User = 1,

        /// <summary>
        /// Expense Tracker Manager.
        /// </summary>
        Manager = 2,

		/// <summary>
		/// Expense Tracker Admin.
		/// </summary>
		SuperUser = 3
    }
}
