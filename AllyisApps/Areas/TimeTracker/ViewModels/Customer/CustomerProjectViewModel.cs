//------------------------------------------------------------------------------
// <copyright file="CustomerProjectViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using AllyisApps.Services.Crm;

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
		public Services.Crm.Customer CustomerInfo { get; internal set; }

		/// <summary>
		/// Gets the list of projects this Customer has.
		/// </summary>
		public IEnumerable<Services.Project.Project> Projects { get; internal set; }
	}
}