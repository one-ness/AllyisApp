using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// organization details view model
	/// </summary>
	public class OrganizationDetailsViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets Organiziton Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets Organizaiton name.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets Site Url.
		/// </summary>
		public string SiteURL { get; set; }

		/// <summary>
		/// Gets or sets Address1 for organizaiton address.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets Country of organization.
		/// </summary>
		public string CountryName { get; set; }

		/// <summary>
		/// Gets or sets State of organization.
		/// </summary>
		public string StateName { get; set; }

		/// <summary>
		/// Gets or sets City of organization.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets PostalCode of organizaton.
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets PhoneNumber of Organization.
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets FaxNumber of organization.
		/// </summary>
		public string FaxNumber { get; set; }

		/// <summary>
		/// side bar view model
		/// </summary>
		public ManageOrgSideBarViewModel SideBarViewModel { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public OrganizationDetailsViewModel()
		{
			this.SideBarViewModel = new ManageOrgSideBarViewModel();
			this.SideBarViewModel.DetailsActive = "active";
		}
	}
}
