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
		/// Gets or sets the OvertimeHours.
		/// </summary>
		public int OvertimeHours { get; set; }

		/// <summary>
		/// Gets or sets the OvertimePeriod.
		/// </summary>
		public string OvertimePeriod { get; set; }

		/// <summary>
		/// Gets or sets the OvertimeMultiplier.
		/// </summary>
		public decimal OvertimeMultiplier { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to use a lock date.
		/// </summary>
		public bool IsLockDateUsed { get; set; }

		/// <summary>
		/// Gets or sets the lock date period (days/weeks/months).
		/// </summary>
		public int LockDatePeriod { get; set; }

		/// <summary>
		/// Gets or sets the lock date quantity.
		/// </summary>
		public int LockDateQuantity { get; set; }

		/// <summary>
		/// Gets or sets the payroll processed date.
		/// All time entries before this date are completely processed
		/// </summary>
		public DateTime PayrollProcessedDate { get; set; }

		/// <summary>
		/// Gets or sets the lock date.
		/// All time entries inbetween the payroll processed date and the lock date are locked (can't be edited)
		/// </summary>
		public DateTime LockDate { get; set; }

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
	}
}