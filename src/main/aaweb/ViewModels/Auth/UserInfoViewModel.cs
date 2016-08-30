//------------------------------------------------------------------------------
// <copyright file="UserInfoViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.ViewModels.Shared;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// Represents basic user info.
	/// </summary>
	public class UserInfoViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the user's info object.
		/// </summary>
		public UserInfo UserInfo { get; set; }

		/// <summary>
		/// Gets or sets the users subscriptions.
		/// </summary>
		public SubscriptionsViewModel Subscriptions { get; set; }

		/// <summary>
		/// Gets or sets A list of pending invitations to organizations.
		/// </summary>
		public List<InvitationViewModel> Invitations { get; set; }
	}

	/// <summary>
	/// Represents a pending invitation to an organization.
	/// </summary>
	public class InvitationViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets The ID for the invitation.
		/// </summary>
		public int InvitationId { get; set; }

		/// <summary>
		/// Gets or sets The ID of the organization associated with the invitation.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets The name of the organization associated with the invitation.
		/// </summary>
		public string OrganizationName { get; set; }
	}
}