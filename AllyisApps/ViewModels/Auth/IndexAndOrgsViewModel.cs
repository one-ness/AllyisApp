using AllyisApps.Services;
using AllyisApps.Services.Lookup;
using System.Collections.Generic;

#pragma warning disable 1591

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// View Model for the Account/Index page.
	/// </summary>
	public class IndexAndOrgsViewModel
	{
		public UserViewModel UserInfo2 { get; set; }

		public List<InvitationViewModel> Invitations { get; set; }

		public List<OrganizationViewModel> Organizations { get; set; }

		/// <summary>
		/// Gets or sets the UserInfo.
		/// </summary>
		public User UserInfo { get; set; }

		/// <summary>
		/// Gets or sets the list of organization/subscription info objects for organizations this user is a member of.
		/// </summary>
		public List<OrgWithSubscriptionsForUserViewModel> OrgInfos { get; set; }

		/// <summary>
		/// Gets or sets the list of InvitationInfos for invitations for this user.
		/// </summary>
		public List<InvitationInfo> InviteInfos { get; set; }

		public IndexAndOrgsViewModel()
		{
			this.Invitations = new List<InvitationViewModel>();
			this.Organizations = new List<OrganizationViewModel>();
			this.UserInfo2 = new UserViewModel();
		}

		/// <summary>
		/// logged in user information
		/// </summary>
		public class UserViewModel
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Email { get; set; }
			public string PhoneNumber { get; set; }
			public string PhoneExtension { get; set; }
			public string Address1 { get; set; }
			public string Address2 { get; set; }
			public string City { get; set; }
			public string State { get; set; }
			public string PostalCode { get; set; }
			public string Country { get; set; }
		}

		/// <summary>
		/// list of invitations pending for the logged in user
		/// </summary>
		public class InvitationViewModel
		{
			public int InvitationId { get; set; }
			public string OrganizationName { get; set; }
		}

		/// <summary>
		/// list of organizations that the user is member of
		/// </summary>
		public class OrganizationViewModel
		{
			public int OrganizationId { get; set; }
			public string OrganizationName { get; set; }
			public string PhoneNumber { get; set; }
			public string PhoneExtension { get; set; }
			public string Address1 { get; set; }
			public string Address2 { get; set; }
			public string City { get; set; }
			public string State { get; set; }
			public string PostalCode { get; set; }
			public string Country { get; set; }
			public string SiteUrl { get; set; }
			public string FaxNumber { get; set; }
			public bool CreateOrganizationAllowed { get; set; }
			public bool CreateSubscriptionAllowed { get; set; }
			public bool ManageOrganizationAllowed { get; set; }
			public List<SubscriptionViewModel> Subscriptions { get; set; }
			public OrganizationViewModel()
			{
				this.Subscriptions = new List<SubscriptionViewModel>();
			}

			/// <summary>
			/// list of this organization's subscription, this user is member of
			/// </summary>
			public class SubscriptionViewModel
			{
				public int SubscriptionId { get; set; }
				public string ProductName { get; set; }
				public string SubscriptionName { get; set; }
			}
		}
	}
}
