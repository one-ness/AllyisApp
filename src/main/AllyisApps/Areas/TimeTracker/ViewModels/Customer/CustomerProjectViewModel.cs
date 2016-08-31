//------------------------------------------------------------------------------
// <copyright file="CustomerProjectViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using AllyisApps.Services.Project;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// ViewModel for displaying Customer and project data.
	/// </summary>
	public class CustomerProjectViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets data about the Customer this model represents.
		/// </summary>
		public CustomerInfo CustomerInfo { get; internal set; }

		/// <summary>
		/// Gets the list of projects this Customer has.
		/// </summary>
		public IEnumerable<ProjectInfo> Projects { get; internal set; }
	}
}