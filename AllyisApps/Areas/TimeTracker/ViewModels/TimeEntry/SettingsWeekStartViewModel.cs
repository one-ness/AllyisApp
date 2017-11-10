//------------------------------------------------------------------------------
// <copyright file="SettingsWeekStartViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for the Settings view.
	/// </summary>
	public class SettingsWeekStartViewModel : SettingsViewModel
	{
		/// <summary>
		/// Gets or sets the StartOfWeek.
		/// </summary>
		public int StartOfWeek { get; set; }
	}
}