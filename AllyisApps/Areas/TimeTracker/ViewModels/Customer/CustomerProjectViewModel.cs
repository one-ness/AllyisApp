//------------------------------------------------------------------------------
// <copyright file="CustomerProjectViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
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
		public Services.Customer CustomerInfo { get; internal set; }

		/// <summary>
		/// Gets the list of projects this Customer has.
		/// </summary>
		public IEnumerable<Services.Project> Projects { get; internal set; }
	}
}
