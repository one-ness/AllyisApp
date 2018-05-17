//------------------------------------------------------------------------------
// <copyright file="CreateApplicationViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.Staffing
{
	/// <summary>
	/// Represents a list of applicants.
	/// </summary>
	public class ApplicantListViewModel
	{
		/// <summary>
		/// Gets or sets the list of applicants.
		/// </summary>
		public List<StaffingApplicantViewModel> Applicants { get; set; }
	}
}