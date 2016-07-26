//-----------------------------------------------------------------------------
// <copyright file="AccountOrgsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.BusinessObjects;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the Account/Organizations view.
	/// </summary>
	public class AccountOrgsViewModel
	{
		/// <summary>
		/// Gets or sets a list of Organizations for display. Used with MVC.
		/// </summary>
		public IEnumerable<OrganizationInfo> Organizations { get; set; }
	}
}