//------------------------------------------------------------------------------
// <copyright file="UserOrgViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// View Model used by the _OrgPanel.
	/// </summary>
	public class UserOrgViewModel
	{
		/// <summary>
		/// Gets or sets UserInfor object for the model.
		/// </summary>
		public User UserInfo { get; set; }

		/// <summary>
		/// Gets or sets Organization subscription info.
		/// </summary>
		public OrgWithSubscriptionsForUserViewModel PanelInfo { get; set; }
	}
}