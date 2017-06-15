//------------------------------------------------------------------------------
// <copyright file="LogOnPartialViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.ViewModels.Shared
{
	/// <summary>
	/// LogOnPartial View Model.
	/// </summary>
	public class LogOnPartialViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the Name of current user.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the Chosen organization name.
		/// </summary>
		public string ChosenOrganizationName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the current user has permission to edit the chosen organization.
		/// </summary>
		public bool CanEditOrganization { get; set; }
	}
}
