//------------------------------------------------------------------------------
// <copyright file="ManageOrgViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.Services.Common.Types;
using System;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// The org management view model.
	/// </summary>
	[CLSCompliant(false)]
	public class ManageOrgViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the organization's Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the organization's details viewmodel.
		/// </summary>
		public OrganizationInfoViewModel Details { get; set; }

		/// <summary>
		/// Gets or sets the organizations members.
		/// </summary>
		public OrganizationMembersViewModel Members { get; set; }

		/// <summary>
		/// Gets or sets the organization's subscriptions.
		/// </summary>
		public IEnumerable<SubscriptionDisplayViewModel> Subscriptions { get; set; }


		/// <summary>
		/// Gets or sets the stripe Token id.
		/// </summary>
		public BillingServicesCustomer BillingCustomer { get; set; }

		/// <summary>
		/// Gets or sets the last four digits of credit card.
		/// </summary>
		public string LastFour { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this customer can edit the organization.
		/// </summary>
		public bool CanEditOrganization { get; set; }

		/// <summary>
		/// Gets or sets the Number of subscriptions this organizaiton has.
		/// </summary>
		public int SubscriptionCount { get; set; }
	}
	/// <summary>
	/// Organizaion View Model with Address Infomation
	/// </summary>
	public class OrganizationInfoViewModel{
		/// <summary>
		/// Organiziton Id 
		/// </summary>
		public int OrganizaitonId { get; set; }

		/// <summary>
		/// Organizaiton name
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Site Url 
		/// </summary>
		public string SiteURL { get; set; }
		
		/// <summary>
		/// Address1 for organizaiton address
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Country of organization.
		/// </summary>
		public string CountryName { get; set; }

		/// <summary>
		/// State of organization.
		/// </summary>
		public string StateName { get; set;}
		/// <summary>
		/// City of organization.
		/// </summary>
		public string City { get; set; }
		/// <summary>
		/// PostalCode of organizaton.
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// PhoneNumber of Organization
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// FaxNumber of organization.
		/// </summary>
		public string FaxNumber { get; set; }
	}
}
