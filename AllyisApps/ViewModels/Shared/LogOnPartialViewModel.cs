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
		/// Gets or sets the Chosen organization id.
		/// </summary>
		public int ChosenOrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Chosen organization name.
		/// </summary>
		public string ChosenOrganizationName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the current user has permission to edit the chosen organization.
		/// </summary>
		public bool CanEditOrganization { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show the organization.
		/// </summary>
		public bool ShowOrganizationPartial { get; set; }

		/// <summary>
		/// Gets or sets a List of the necessary information about the organizations the current user is a member of.
		/// </summary>
		public List<OrganizationBriefInfo> UserOrganizationBriefInfoList { get; set; }
	}

	/// <summary>
	/// Just the necessary information about an organization for its listing on the LogOnPartial.
	/// </summary>
	public class OrganizationBriefInfo
	{
		// public string AreaURL { get; set; } //removed because it seems like we just sorta circumvented all the logic this was used for and now this is useless

		/// <summary>
		/// Gets or sets th OrganizationID, used for obtaining the AreaURL above.
		/// </summary>
		public int OrganizationID { get; set; }

		/// <summary>
		/// Gets or sets The name of the organization.
		/// </summary>
		public string OrganizationName { get; set; }
	}
}
