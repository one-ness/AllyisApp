﻿using System.Collections.Generic;

#pragma warning disable 1591

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// View model for /account/index.
	/// </summary>
	public class AccountIndexViewModel : BaseViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AccountIndexViewModel"/> class.
		/// </summary>
		public AccountIndexViewModel()
		{
			Invitations = new List<InvitationViewModel>();
			Organizations = new List<OrganizationViewModel>();
			UserInfo = new UserViewModel();
		}

		/// <summary>
		/// Gets or sets the user model.
		/// </summary>
		public UserViewModel UserInfo { get; set; }

		/// <summary>
		/// Gets or sets the invitations model list.
		/// </summary>
		public List<InvitationViewModel> Invitations { get; set; }

		/// <summary>
		/// Gets or sets the organization model list.
		/// </summary>
		public List<OrganizationViewModel> Organizations { get; set; }

		/// <summary>
		/// Logged in user information.
		/// </summary>
		public class UserViewModel
		{
			/// <summary>
			/// Gets or sets FirstName.
			/// </summary>
			public string FirstName { get; set; }

			/// <summary>
			/// Gets or sets LastName.
			/// </summary>
			public string LastName { get; set; }

			/// <summary>
			/// Gets or sets Email.
			/// </summary>
			public string Email { get; set; }

			/// <summary>
			/// Gets or sets PhoneNumber.
			/// </summary>
			public string PhoneNumber { get; set; }

			/// <summary>
			/// Gets or sets PhoneExtension.
			/// </summary>
			public string PhoneExtension { get; set; }

			/// <summary>
			/// Gets or sets Address1.
			/// </summary>
			public string Address1 { get; set; }

			/// <summary>
			/// Gets or sets Address2.
			/// </summary>
			public string Address2 { get; set; }

			/// <summary>
			/// Gets or sets City.
			/// </summary>
			public string City { get; set; }

			/// <summary>
			/// Gets or sets State.
			/// </summary>
			public string State { get; set; }

			/// <summary>
			/// Gets or sets PostalCode.
			/// </summary>
			public string PostalCode { get; set; }

			/// <summary>
			/// Gets or sets Country.
			/// </summary>
			public string Country { get; set; }

			/// <summary>
			/// Gets or sets variable to indicate if convert to employer account option must be shown
			/// </summary>
			public bool ShowConvertToEmployerAccount { get; set; }
		}

		/// <summary>
		/// List of invitations pending for the logged in user.
		/// </summary>
		public class InvitationViewModel
		{
			/// <summary>
			/// Gets or sets the invitation id.
			/// </summary>
			public int InvitationId { get; set; }

			/// <summary>
			/// Gets or sets the organization name.
			/// </summary>
			public string OrganizationName { get; set; }
		}

		/// <summary>
		/// List of organizations that the user is member of.
		/// </summary>
		public class OrganizationViewModel
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="OrganizationViewModel"/> class.
			/// </summary>
			public OrganizationViewModel()
			{
				Subscriptions = new List<SubscriptionViewModel>();
			}

			/// <summary>
			/// Gets or sets the OrganizationId.
			/// </summary>
			public int OrganizationId { get; set; }

			/// <summary>
			/// Gets or sets the OrganizationId.
			/// </summary>
			public string OrganizationName { get; set; }

			/// <summary>
			/// Gets or sets PhoneNumber.
			/// </summary>
			public string PhoneNumber { get; set; }

			/// <summary>
			/// Gets or sets Address1.
			/// </summary>
			public string Address1 { get; set; }

			/// <summary>
			/// Gets or sets Address2.
			/// </summary>
			public string Address2 { get; set; }

			/// <summary>
			/// Gets or sets City.
			/// </summary>
			public string City { get; set; }

			/// <summary>
			/// Gets or sets State.
			/// </summary>
			public string State { get; set; }

			/// <summary>
			/// Gets or sets PostalCode.
			/// </summary>
			public string PostalCode { get; set; }

			/// <summary>
			/// Gets or sets Country.
			/// </summary>
			public string Country { get; set; }

			/// <summary>
			/// Gets or sets the SiteUrl.
			/// </summary>
			public string SiteUrl { get; set; }

			/// <summary>
			/// Gets or sets the FaxNumber.
			/// </summary>
			public string FaxNumber { get; set; }

			public bool IsReadOrgDetailsAllowed { get; set; }

			public bool IsReadMembersListAllowed { get; set; }

			public bool IsReadSubscriptionsListAllowed { get; set; }

			public bool IsReadBillingDetailsAllowed { get; set; }

			public bool IsReadPermissionsListAllowed { get; set; }

			public bool IsCreateSubscriptionAllowed { get; set; }

			/// <summary>
			/// Gets or sets the subscriptions model list.
			/// </summary>
			public List<SubscriptionViewModel> Subscriptions { get; set; }

			/// <summary>
			/// List of this organization's subscription, that user is member of.
			/// </summary>
			public class SubscriptionViewModel
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="SubscriptionViewModel"/> class.
				/// </summary>
				public SubscriptionViewModel()
				{
				}

				/// <summary>
				/// Gets or sets the ProductDescription.
				/// </summary>
				public string ProductDescription { get; set; }

				/// <summary>
				/// Gets or sets the SubscriptionId.
				/// </summary>
				public int SubscriptionId { get; set; }

				/// <summary>
				/// Gets or sets the ProductName.
				/// </summary>
				public string ProductName { get; set; }

				/// <summary>
				/// Gets or sets the SubscriptionName.
				/// </summary>
				public string SubscriptionName { get; set; }

				/// <summary>
				/// Gets or sets the AreaUrl.
				/// </summary>
				public string AreaUrl { get; set; }

				/// <summary>
				/// Gets or sets the ProductId.
				/// </summary>
				public Services.Billing.ProductIdEnum ProductId { get; set; }

				/// <summary>
				/// Gets or sets the ProductGoToUrl.
				/// </summary>
				public string ProductGoToUrl { get; set; }

				/// <summary>
				/// Gets or sets the IconUrl.
				/// </summary>
				public string IconUrl { get; set; }
			}
		}
	}
}