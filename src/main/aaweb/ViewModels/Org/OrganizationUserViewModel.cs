//------------------------------------------------------------------------------
// <copyright file="OrganizationUserViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// View model for one item in the _OrgMembers partial on Account/Manage. Part of OrganizationMembersViewModel.
	/// </summary>
	public class OrganizationUserViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the employee Id.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the full name of the user.
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// Gets or sets the organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the user's permissions in string readable format.  Used as a key for resource localization.
		/// </summary>
		public string PermissionLevel { get; set; }

		/// <summary>
		/// Gets or sets the user Id.
		/// </summary>
		public int UserId { get; set; }
	}
}