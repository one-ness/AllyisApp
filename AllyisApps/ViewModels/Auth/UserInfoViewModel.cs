//------------------------------------------------------------------------------
// <copyright file="UserInfoViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using AllyisApps.Services;
using AllyisApps.ViewModels.Shared;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents basic user info.
	/// </summary>
	public class UserInfoViewModel
	{
		/// <summary>
		/// Gets or sets the user's info object.
		/// </summary>
		public User UserInfo { get; set; }

		/// <summary>
		/// Gets or sets the users subscriptions.
		/// </summary>
		public SubscriptionsViewModel Subscriptions { get; set; }

		/// <summary>
		/// Gets or sets A list of pending invitations to organizations.
		/// </summary>
		public List<InvitationViewModel> Invitations { get; set; }
	}
}
