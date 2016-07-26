//------------------------------------------------------------------------------
// <copyright file="OrganizationDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Represents the Organizations table in the database.
	/// </summary>
	public class OrganizationDBEntity : BasePoco
	{
		private int pOrganizationId;
		private string pName;
		private string pSiteUrl;
		private string pAddress;
		private string pCity;
		private string pState;
		private string pCountry;
		private string pPostalCode;
		private string pPhoneNumber;
		private string pFaxNumber;
		private DateTime pDateCreated;
		private string pSubdomain;

		/// <summary>
		/// Gets or sets the organization's ID.
		/// </summary>
		public int OrganizationId
		{
			get
			{
				return this.pOrganizationId;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, int>(ref this.pOrganizationId, (OrganizationDBEntity x) => x.OrganizationId, value);
			}
		}

		/// <summary>
		/// Gets or sets the organization's e-mail address.
		/// </summary>
		public string Name
		{
			get
			{
				return this.pName;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pName, (OrganizationDBEntity x) => x.Name, value);
			}
		}

		/// <summary>
		/// Gets or sets the organization's website URL.
		/// </summary>
		[DisplayName("Website")]
		public string SiteUrl
		{
			get
			{
				return this.pSiteUrl;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pSiteUrl, (OrganizationDBEntity x) => x.SiteUrl, value);
			}
		}

		/// <summary>
		/// Gets or sets the organization's physical address.
		/// </summary>
		public string Address
		{
			get
			{
				return this.pAddress;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pAddress, (OrganizationDBEntity x) => x.Address, value);
			}
		}

		/// <summary>
		/// Gets or sets the organization's city.
		/// </summary>
		public string City
		{
			get
			{
				return this.pCity;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pCity, (OrganizationDBEntity x) => x.City, value);
			}
		}

		/// <summary>
		/// Gets or sets the organization's state.
		/// </summary>
		public string State
		{
			get
			{
				return this.pState;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pState, (OrganizationDBEntity x) => x.State, value);
			}
		}

		/// <summary>
		/// Gets or sets the organization's country.
		/// </summary>
		public string Country
		{
			get
			{
				return this.pCountry;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pCountry, (OrganizationDBEntity x) => x.Country, value);
			}
		}

		/// <summary>
		/// Gets or sets the organization's postal code.
		/// </summary>
		[DisplayName("Postal Code")]
		public string PostalCode
		{
			get
			{
				return this.pPostalCode;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pPostalCode, (OrganizationDBEntity x) => x.PostalCode, value);
			}
		}

		/// <summary>
		/// Gets or sets the organization's phone number.
		/// </summary>
		[DisplayName("Phone Number")]
		public string PhoneNumber
		{
			get
			{
				return this.pPhoneNumber;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pPhoneNumber, (OrganizationDBEntity x) => x.PhoneNumber, value);
			}
		}

		/// <summary>
		/// Gets or sets the organization's fax number.
		/// </summary>
		[DisplayName("Fax Number")]
		public string FaxNumber
		{
			get
			{
				return this.pFaxNumber;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pFaxNumber, (OrganizationDBEntity x) => x.FaxNumber, value);
			}
		}

		/// <summary>
		/// Gets or sets DateCreated.
		/// </summary>
		[DisplayName("Date Created")]
		public DateTime CreatedUTC
		{
			get
			{
				return this.pDateCreated;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, DateTime>(ref this.pDateCreated, (OrganizationDBEntity x) => x.CreatedUTC, value);
			}
		}
		
		/// <summary>
		/// Gets or sets subdomain.
		/// </summary>
		public string Subdomain
		{
			get
			{
				return this.pSubdomain;
			}

			set
			{
				this.ApplyPropertyChange<OrganizationDBEntity, string>(ref this.pSubdomain, (OrganizationDBEntity x) => x.Subdomain, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user has permissions to edit organization.
		/// </summary>
		public bool CanEditOrganization
		{
			get;
			////{ AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization); }
			set;
		}
	}
}
