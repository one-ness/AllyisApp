//------------------------------------------------------------------------------
// <copyright file="SettingsOvertimeViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for the Settings view.
	/// </summary>
	public class SettingsOvertimeViewModel : SettingsViewModel
	{
		/// <summary>
		/// Gets or sets the OvertimeHours.
		/// </summary>
		public int? OvertimeHours { get; set; }

		/// <summary>
		/// Gets or sets the OvertimePeriod.
		/// </summary>
		public string OvertimePeriod { get; set; }

		/// <summary>
		/// Bool for whether or not overtime is being used by the company.
		/// Also equal to overtimeHours != null
		/// </summary>
		public bool IsOvertimeUsed { get; set; }

		/// <summary>
		/// Gets or sets a dictionary containing all the options + values for overtime period
		/// </summary>
		public Dictionary<string, string> OvertimePeriodOptions { get; set; }
	}
}