//------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for the Settings Pay Class view tab.
	/// </summary>
	public class SettingsPayClassesViewModel : SettingsViewModel
	{
		/// <summary>
		/// Gets or sets the pay classes for an organization.
		/// </summary>
		public IEnumerable<PayClassViewModel> PayClasses { get; set; }

		/// <summary>
		/// Pay classes View for settings page.
		/// </summary>
		public class PayClassViewModel
		{
			/// <summary>
			/// Default constructor -- these fields should always be initialized
			/// </summary>
			/// <param name="name"></param>
			/// <param name="id"></param>
			public PayClassViewModel(string name, int id)
			{
				PayClassName = name;
				PayClassId = id;
			}

			/// <summary>
			/// Gets or sets Name of pay Class.
			/// </summary>
			public string PayClassName { get; set; }

			/// <summary>
			/// Gets or sets id of pay Class.
			/// </summary>
			public int PayClassId { get; set; }
		}
	}
}