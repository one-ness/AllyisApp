//------------------------------------------------------------------------------
// <copyright file="Setting.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.TimeTracker
{
	/// <summary>
	/// An object for keeping track of the settings for a time tracker.
	/// </summary>
	public class Setting
	{
		/// <summary>
		/// Gets or sets the OrganizationId.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the StartOfWeek.
		/// </summary>
		public int StartOfWeek { get; set; }

		/// <summary>
		/// Gets or sets the OvertimeHours. Null means that the org isn't using overtime.
		/// </summary>
		public int? OvertimeHours { get; set; }

		/// <summary>
		/// Gets or sets the OvertimePeriod.
		/// </summary>
		public string OvertimePeriod { get; set; }

		/// <summary>
		/// Gets or sets the payroll processed date.
		/// All time entries before this date are completely processed
		/// </summary>
		public DateTime? PayrollProcessedDate { get; set; }

		/// <summary>
		/// Gets or sets the lock date.
		/// All time entries inbetween the payroll processed date and the lock date are locked (can't be edited)
		/// </summary>
		public DateTime? LockDate { get; set; }

		/// <summary>
		/// Gets or sets the Pay period.
		/// This is a JSON string containing all info necessary to calculate the pay period.
		/// It is one of two objects:
		/// 
		/// For a duration type pay period, the string will be like:
		/// '{
		///     "type": "duration",
		///     "duration": "14",
		///     "startDate": "2017-10-16"
		///  }'
		/// 
		/// For a dates type pay period, the string will be like:
		/// '{
		///     "type": "dates",
		///     "dates": [1, 16]
		///  }'
		/// </summary>
		public string PayPeriod { get; set; }

		/// <summary>
		/// Bool for whether or not the overtime settings were recently changed
		/// </summary>
		/// <remarks>
		/// If this property is true, that means that the org currently has time entries
		/// inbetween its payroll process date and its lock date that have not been
		/// updated to new overtime settings due to the fact that they are locked.
		/// Any reduction of lock date therefore must endure additional validation to ensure
		/// that no overtime periods contain a mixture of locked old ot setting entries and
		/// new ot setting entries, which would make overtime calculation impossible.
		/// 
		/// If this property is false, no additional validation for reducing lock date is necessary.
		/// </remarks>
		public bool OtSettingRecentlyChanged { get; set; }
	}
}