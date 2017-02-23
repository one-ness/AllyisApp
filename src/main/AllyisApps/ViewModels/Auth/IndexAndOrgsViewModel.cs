using AllyisApps.Services;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// View Model for the Account/Index page.
	/// </summary>
	public class IndexAndOrgsViewModel
	{
		/// <summary>
		/// Gets or sets the UserInfo.
		/// </summary>
		public UserInfo UserInfo { get; set; }

		/// <summary>
		/// Gets or sets the list of organization/subscription info objects for organizations this user is a member of.
		/// </summary>
		public List<OrgWithSubscriptionsForUserViewModel> OrgInfos { get; set; }

		/// <summary>
		/// Gets or sets the list of InvitationInfos for invitations for this user.
		/// </summary>
		public List<InvitationInfo> InviteInfos { get; set; }

	}
}
