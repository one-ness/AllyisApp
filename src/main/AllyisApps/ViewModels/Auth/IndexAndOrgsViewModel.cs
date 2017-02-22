using AllyisApps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// 
	/// </summary>
	public class IndexAndOrgsViewModel
	{
		/// <summary>
		/// Gets or sets the view model for user profile information.
		/// </summary>
		public UserInfoViewModel UserModel { get; set; }

		/// <summary>
		/// Gets or sets the view model for organizations information.
		/// </summary>
		public AccountOrgsViewModel OrgModel { get; set; }

		/// <summary>
		/// Gets or sets the user information.
		/// </summary>
		public UserInfo UserInfo { get; set; }

		/// <summary>
		/// Gets or sets the organization info for each of the user's organizations.
		/// </summary>
		public List<OrganizationInfo> OrgInfos { get; set; }

		/// <summary>
		/// Gets or sets the invitation info for each of the user's open invitations.
		/// </summary>
		public List<InvitationInfo> InviteInfos { get; set; }
	}
}