//------------------------------------------------------------------------------
// <copyright file="EditSubscriptionUsersViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

using AllyisApps.Services.BusinessObjects;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// Represents basic user info.
	/// </summary>
	public class EditSubscriptionUsersViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the maximum number of users as defined by subscription license.
		/// </summary>
		public int MaxUsers { get; set; }

		/// <summary>
		/// Gets details of the subscription.
		/// </summary>
		public SubscriptionInfo Details { get; internal set; }

		/// <summary>
		/// Gets possible roles for the product the user is subscribed to.
		/// </summary>
		public IEnumerable<SubscriptionRoleInfo> Roles { get; internal set; }

		/// <summary>
		/// Gets or sets list of all users in the organization this subscription is used by.
		/// </summary>
		public IEnumerable<SubscriptionUserInfo> Users { get; set; }

		/// <summary>
		/// Gets the number of invitations where the user was given a role in TimeTracker.
		/// </summary>
		public int InvitationCount { get; internal set; }

		/// <summary>
		/// Gets a value indicating whether the Model was able to properly initialize or not.
		/// </summary>
		public bool IsValid { get; internal set; }
	}
}