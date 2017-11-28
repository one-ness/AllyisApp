using System;
using System.Collections.Generic;
using AllyisApps.Services.Crm;

namespace AllyisApps.ViewModels.TimeTracker.Project
{
	/// <summary>
	///	View model for the project View.
	/// </summary>
	public class ProjectsViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets a list of projects.
		/// </summary>
		public List<CompleteProjectViewModel> Projects { get; set; }

		/// <summary>
		/// Gets or sets whether the projects are being displayed for a single customer.
		/// </summary>
		public bool ForCustomer { get; internal set; }
	}
}