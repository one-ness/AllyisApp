//------------------------------------------------------------------------------
// <copyright file="CustomerProjectViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.Customer
{
	/// <summary>
	/// ViewModel for displaying Customer and project data.
	/// </summary>
	public class CustomerProjectViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets data about the Customer this model represents.
		/// </summary>
		public CustomerViewModel CustomerInfo { get; internal set; }

		/// <summary>
		/// Gets the list of projects this Customer has.
		/// </summary>
		public IEnumerable<ProjectViewModel> Projects { get; internal set; }

		/// <summary>
		/// Customer View Model.
		/// </summary>
		public class CustomerViewModel
		{
			/// <summary>
			/// Gets or sets Is Acive.
			/// </summary>
			public bool? IsActive { get; set; }

			/// <summary>
			/// Gets or sets Cusomter Id.
			/// </summary>
			public int CustomerId { get; set; }

			/// <summary>
			/// Gets or sets Cusomter Name.
			/// </summary>
			public string CustomerName { get; set; }
		}

		/// <summary>
		/// Internal Project View Model.
		/// </summary>
		public class ProjectViewModel
		{
			/// <summary>
			/// Gets  ProjectId.
			/// </summary>
			public int ProjectId { get; internal set; }

			/// <summary>
			/// Gets  Name of project.
			/// </summary>
			public string ProjectName { get; internal set; }

			/// <summary>
			/// Gets Custoemr Project is assigned to.
			/// </summary>
			public int CustomerId { get; internal set; }

			/// <summary>
			/// Gets  Organization Project is assigned to.
			/// </summary>
			public int OrganizationId { get; internal set; }
		}
	}
}