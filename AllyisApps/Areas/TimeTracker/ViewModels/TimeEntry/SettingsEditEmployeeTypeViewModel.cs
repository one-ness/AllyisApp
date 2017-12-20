

using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	///
	/// </summary>
	public class SettingsEditEmployeeTypeViewModel
	{
		/// <summary>
		///
		/// </summary>
		public bool IsEdit { get; set; }

		/// <summary>
		///
		/// </summary>
		public int EmployeeTypeId { get; set; }

		/// <summary>
		///
		/// </summary>
		public string EmployeeTypeName { get; set; }

		/// <summary>
		///
		/// </summary>
		public List<PayClassInfoViewModel> PayClasses { get; set; }

		/// <summary>
		///
		/// </summary>
		public List<PayClassInfoViewModel> CurrentPayClasses { get; set; }

		/// <summary>
		///
		/// </summary>
		public int[] SelectedPayClass { get; set; }
	}
}