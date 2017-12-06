using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
		public List<PayClassInfo> PayClasses { get; set; }

		/// <summary>
		///
		/// </summary>
		public List<PayClassInfo> CurrentPayClasses { get; set; }

		/// <summary>
		///
		/// </summary>
		public int[] SelectedPayClass { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class PayClassInfo
	{
		/// <summary>
		///
		/// </summary>
		public int PayClassId { get; set; }

		/// <summary>
		///
		/// </summary>
		public string PayClassName { get; set; }
	}

}