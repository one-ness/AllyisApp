//------------------------------------------------------------------------------
// <copyright file="ProductRoleEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// Role of the user in an organization.
	/// </summary>
	public enum OrganizationRoleEnum
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

	public enum StaffingManagerRole
	{
		/// <summary>
		/// Staffing Manager Unavailable.
		/// </summary>
		NotInProduct = 0,

		/// <summary>
		/// Staffing Manager User.
		/// </summary>
		User = 1,

		/// <summary>
		/// Staffing Manager Manager.
		/// </summary>
		Manager = 2
	}

	/// <summary>
	/// Product role aka Subscription role.
	/// </summary>
	public enum TimeTrackerRole
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

	public enum ExpenseTrackerRole
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