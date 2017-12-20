

using System.Collections.Generic;

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
		public List<EmployeeTypeViewModel> EmployeeTypes { get; set; }
	}

	/// <summary>
	/// View Model for employee type.
	/// </summary>
	public class EmployeeTypeViewModel
	{
		/// <summary>
		/// Gets or sets the Employee type id.
		/// </summary>
		public int EmployeeTypeId { get; set; }

		/// <summary>
		/// Gets or sets the organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the employee type name.
		/// </summary>
		public string EmployeeTypeName { get; set; }
	}
}