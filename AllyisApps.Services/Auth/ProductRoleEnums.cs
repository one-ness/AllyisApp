//------------------------------------------------------------------------------
// <copyright file="ProductRoleEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// list of builtin roles
	/// </summary>
	public enum BuiltinRoleEnum : int
	{
		/// <summary>
		/// not in product
		/// </summary>
		NotInProduct = 0,

		/// <summary>
		/// administrator
		/// </summary>
		Admin = 1,

		/// <summary>
		/// user
		/// </summary>
		User = 2,

		/// <summary>
		/// custom
		/// </summary>
		Custom = 4,
	}

	/// <summary>
	/// Role of the user in an organization.
	/// </summary>
	public enum OrganizationRoleEnum
	{
		/// <summary>
		/// Organization admin (aka owner).
		/// </summary>
		Admin = 1,

		/// <summary>
		/// Organization member.
		/// </summary>
		Member = 2,
		
		/// <summary>
		/// custom role
		/// </summary>
		Custom = 4,
	}

	public enum StaffingManagerRole
	{
		/// <summary>
		/// Staffing Manager Unavailable.
		/// </summary>
		NotInProduct = 0,

		/// <summary>
		/// Staffing Manager admin.
		/// </summary>
		Admin = 1,

		/// <summary>
		/// Staffing Manager user.
		/// </summary>
		User = 2
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
		/// TimeTracker admin.
		/// </summary>
		Admin = 1,

		/// <summary>
		/// TimeTracker user.
		/// </summary>
		User = 2,

		/// <summary>
		/// approver
		/// </summary>
		Approver = 4,
	}

	public enum ExpenseTrackerRole
	{
		/// <summary>
		/// Expense Tracker Unavailable.
		/// </summary>
		NotInProduct = 0,

		/// <summary>
		/// Expense Tracker admin.
		/// </summary>
		Admin = 1,

		/// <summary>
		/// Expense Tracker Manager.
		/// </summary>
		Manager = 2,

		/// <summary>
		/// Expense Tracker user.
		/// </summary>
		User = 3
	}
}