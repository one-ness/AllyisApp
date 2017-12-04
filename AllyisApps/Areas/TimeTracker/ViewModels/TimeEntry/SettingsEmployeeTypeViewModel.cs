using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	///
	/// </summary>
	public class SettingsEmployeeTypeViewModel : SettingsViewModel
	{
		/// <summary>
		///
		/// </summary>
		public List<EmployeeType> EmployeeTypes { get; set; }
	}
}