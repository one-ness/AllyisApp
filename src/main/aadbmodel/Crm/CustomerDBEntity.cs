//------------------------------------------------------------------------------
// <copyright file="CustomerDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel;

namespace AllyisApps.DBModel.Crm
{
	/// <summary>
	/// Represents the Users table in the database.
	/// </summary>
	public class CustomerDBEntity : BasePoco
	{
		private int cCustomerId;
		private string cName;
		private string cAddress;
		private string cCity;
		private string cState;
		private string cCountry;
		private string cPostalCode;
		private string cContactEmail;
		private string cContactPhoneNumber;
		private string cFaxNumber;
		private string cWebsite;
		private string cEIN;
		private string cDateCreated;
		private int cOrganizationId;

		/// <summary>
		/// Gets or sets CustomerId.
		/// </summary>
		public int CustomerId
		{
			get
			{
				return this.cCustomerId;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, int>(ref this.cCustomerId, (CustomerDBEntity x) => x.cCustomerId, value);
			}
		}

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		[DisplayName("Name")]
		public string Name
		{
			get
			{
				return this.cName;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cName, (CustomerDBEntity x) => x.cName, value);
			}
		}

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
		[DisplayName("Address")]
		public string Address
		{
			get
			{
				return this.cAddress;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cAddress, (CustomerDBEntity x) => x.cAddress, value);
			}
		}

		/// <summary>
		/// Gets or sets City.
		/// </summary>
		public string City
		{
			get
			{
				return this.cCity;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cCity, (CustomerDBEntity x) => x.cCity, value);
			}
		}

		/// <summary>
		/// Gets or sets State.
		/// </summary>
		public string State
		{
			get
			{
				return this.cState;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cState, (CustomerDBEntity x) => x.cState, value);
			}
		}

		/// <summary>
		/// Gets or sets Country.
		/// </summary>
		public string Country
		{
			get
			{
				return this.cCountry;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cCountry, (CustomerDBEntity x) => x.cCountry, value);
			}
		}

		/// <summary>
		/// Gets or sets PostalCode.
		/// </summary>
		[DisplayName("Postal Code")]
		public string PostalCode
		{
			get
			{
				return this.cPostalCode;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cPostalCode, (CustomerDBEntity x) => x.cPostalCode, value);
			}
		}

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		public string ContactEmail
		{
			get
			{
				return this.cContactEmail;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cContactEmail, (CustomerDBEntity x) => x.cContactEmail, value);
			}
		}

		/// <summary>
		/// Gets or sets PhoneNumber.
		/// </summary>
		[DisplayName("Phone Number")]
		public string ContactPhoneNumber
		{
			get
			{
				return this.cContactPhoneNumber;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cContactPhoneNumber, (CustomerDBEntity x) => x.cContactPhoneNumber, value);
			}
		}

		/// <summary>
		/// Gets or sets Fax number.
		/// </summary>
		[DisplayName("Fax Number")]
		public string FaxNumber
		{
			get
			{
				return this.cFaxNumber;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cFaxNumber, (CustomerDBEntity x) => x.cFaxNumber, value);
			}
		}

		/// <summary>
		/// Gets or sets Fax number.
		/// </summary>
		[DisplayName("Website")]
		public string Website
		{
			get
			{
				return this.cWebsite;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cWebsite, (CustomerDBEntity x) => x.cWebsite, value);
			}
		}

		/// <summary>
		/// Gets or sets Employer Identification Number.
		/// </summary>
		[DisplayName("EIN")]
		public string EIN
		{
			get
			{
				return this.cEIN;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cEIN, (CustomerDBEntity x) => x.cEIN, value);
			}
		}

		/// <summary>
		/// Gets or sets the date the customer was created.
		/// </summary>
		[DisplayName("Created UTC")]
		public string CreatedUTC
		{
			get
			{
				return this.cDateCreated;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, string>(ref this.cDateCreated, (CustomerDBEntity x) => x.cDateCreated, value);
			}
		}

		/// <summary>
		/// Gets or sets the id of the organization associated with the customer.
		/// </summary>
		[DisplayName("OrganizationID")]
		public int OrganizationId
		{
			get
			{
				return this.cOrganizationId;
			}

			set
			{
				this.ApplyPropertyChange<CustomerDBEntity, int>(ref this.cOrganizationId, (CustomerDBEntity x) => x.cOrganizationId, value);
			}
		}
	}
}