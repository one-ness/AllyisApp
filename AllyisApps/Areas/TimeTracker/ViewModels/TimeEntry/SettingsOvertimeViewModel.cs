//------------------------------------------------------------------------------
// <copyright file="SettingsOvertimeViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for the Settings view.
	/// </summary>
	public class SettingsOvertimeViewModel : SettingsViewModel
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
		/// Get or sets todays date for holiday datepicker
		/// </summary>
		public DateTime Today { get; set; }
	}
}