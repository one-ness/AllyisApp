//------------------------------------------------------------------------------
// <copyright file="ManageCustomerViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.Customer
{
	/// <summary>
	/// ViewModel for the Customer management page.
	/// </summary>
	public class ManageCustomerViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets the list of Customers and their project data.
		/// </summary>
		public IEnumerable<CustomerProjectViewModel> Customers { get; internal set; }

		/// <summary>
		/// Gets the id of the current organization.
		/// </summary>
		public int OrganizationId { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user can edit customers/projects.
		/// </summary>
		public bool canEdit { get; set; }
	}
}
