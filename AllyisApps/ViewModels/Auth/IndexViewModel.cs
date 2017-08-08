﻿using AllyisApps.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#pragma warning disable 1591

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// view model for /account/index
	/// </summary>
	public class AccountIndexViewModel : BaseViewModel
	{
		public UserViewModel UserInfo { get; set; }
		public List<InvitationViewModel> Invitations { get; set; }
		public List<OrganizationViewModel> Organizations { get; set; }

		public AccountIndexViewModel()
		{
			this.Invitations = new List<InvitationViewModel>();
			this.Organizations = new List<OrganizationViewModel>();
			this.UserInfo = new UserViewModel();
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
		/// list of organizations that the user is member of
		/// </summary>
		public class OrganizationViewModel
		{
			public int OrganizationId { get; set; }
			public string OrganizationName { get; set; }
			public string PhoneNumber { get; set; }
			public string PhoneExtension { get; set; }
			public string StreetAddress { get; set; }
			public string City { get; set; }
			public string State { get; set; }
			public string PostalCode { get; set; }
			public string Country { get; set; }
			public string SiteUrl { get; set; }
			public string FaxNumber { get; set; }
			public bool IsManageAllowed { get; set; }
			public List<SubscriptionViewModel> Subscriptions { get; set; }
			public OrganizationViewModel()
			{
				this.Subscriptions = new List<SubscriptionViewModel>();
			}

			/// <summary>
			/// list of this organization's subscription, that user is member of
			/// </summary>
			public class SubscriptionViewModel
			{
				public int SubscriptionId { get; set; }
				public string ProductName { get; set; }
                public string ProductDescription { get; set; }
				public string SubscriptionName { get; set; }
                public string AreaUrl { get; set; }
                public ProductIdEnum productID { get; set; }
                public int? startDate { get; set; }
                public int? endDate { get; set; }
                
			}
		}
	}
}
