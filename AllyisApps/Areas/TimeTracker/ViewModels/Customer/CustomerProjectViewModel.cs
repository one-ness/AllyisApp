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
		/// 
		/// </summary>
		public class CustomerViewModel
		{
			/// <summary>
			/// Is Acive
			/// </summary>
			public bool? IsActive;
			/// <summary>
			/// CusomterId
			/// </summary>
			public int CustomerId;

			/// <summary>
			/// Cusomter Name
			/// </summary>
			public string CustomerName;
		}

		/// <summary>
		/// Gets the list of projects this Customer has.
		/// </summary>
		public IEnumerable<ProjectViewModel> Projects { get; internal set; }

		/// <summary>
		/// 
		/// </summary>
		public class ProjectViewModel
		{
			public int ProjectId;
			public string ProjectName;

			public int CustomerId { get; internal set; }
			public int OrganizationId { get; internal set; }
		}
	}
}
