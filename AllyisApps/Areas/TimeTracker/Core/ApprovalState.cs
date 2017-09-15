//------------------------------------------------------------------------------
// <copyright file="ApprovalState.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Areas.TimeTracker.Core
{
	/// <summary>
	/// Representation of all available approval states.
	/// </summary>
	public enum ApprovalState
	{
		/// <summary>
		/// The default state of a Time Entry. No status on approval.
		/// </summary>
		NoApprovalState = 0,

		/// <summary>
		/// The time entry has been tagged by a manager as not approved.
		/// </summary>
		NotApproved = 1,

		/// <summary>
		/// The time entry has been tagged by a manager as approved.
		/// </summary>
		Approved = 2
	}
}