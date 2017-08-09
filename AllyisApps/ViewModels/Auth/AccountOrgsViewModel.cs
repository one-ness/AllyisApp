//-----------------------------------------------------------------------------
// <copyright file="AccountOrgsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using AllyisApps.ViewModels.Shared;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the Account/Organizations view.
	/// </summary>
	public class AccountOrgsViewModel
	{
		/// <summary>
		/// Gets or sets a list of Organizations and this user's subscription info in each for display.
		/// </summary>
		public IEnumerable<SubscriptionsViewModel> Organizations { get; set; }
	}
}
