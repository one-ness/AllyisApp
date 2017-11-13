//------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for the Settings view.
	/// </summary>
	public class SettingsHolidayViewModel : SettingsViewModel
	{
		/// <summary>
		/// Gets or sets the holidays for an organization.
		/// </summary>
		public IEnumerable<HolidayViewModel> Holidays { get; set; }

		/// <summary>
		/// Holiday View Model for settings page.
		/// </summary>
		public class HolidayViewModel
		{
			/// <summary>
			/// Gets or sets Holiday Date.
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// Gets or setsHoliday Name.
			/// </summary>
			public string HolidayName { get; set; }

			/// <summary>
			/// Gets or sets Holiday Id.
			/// </summary>
			public int HolidayId { get; set; }
		}
	}
}