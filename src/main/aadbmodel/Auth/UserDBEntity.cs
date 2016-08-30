//------------------------------------------------------------------------------
// <copyright file="UserDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace AllyisApps.DBModel.Auth
{
	/// <summary>
	/// Represents the Users table in the database.
	/// </summary>
	public class UserDBEntity : BasePoco
	{ // TODO: Add language field once we have language in DB
		private int pUserId;

		////private string pAspnetUserId;
		private string pFirstName;

		private string pLastName;
		private DateTime? pDateOfBirth;
		private string pAddress;
		private string pCity;
		private string pState;
		private string pCountry;
		private string pPostalCode;
		private string pEmail;
		private string pPhoneNumber;
		private string pPhoneExtension;
		private int pLastSubscriptionId;
		private string pUserName;
		private bool pEmailConfirmed;
		private bool pPhoneNumberConfirmed;
		private bool pTwoFactorEnabled;
		private int pAccessFailedCount;
		private bool pLockoutEnabled;
		private int pActiveOrganizationId;
		private int pLanguagePreference;

		private DateTime? pLockoutEndDateUtc;
		private string pPasswordHash;

		private Guid? passwordResetCode;

		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId
		{
			get
			{
				return this.pUserId;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, int>(ref this.pUserId, (UserDBEntity x) => x.UserId, value);
			}
		}

		/// <summary>
		/// Gets or sets FirstName.
		/// </summary>
		[DisplayName("First Name")]
		public string FirstName
		{
			get
			{
				return this.pFirstName;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pFirstName, (UserDBEntity x) => x.FirstName, value);
			}
		}

		/// <summary>
		/// Gets or sets LastName.
		/// </summary>
		[DisplayName("Last Name")]
		public string LastName
		{
			get
			{
				return this.pLastName;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pLastName, (UserDBEntity x) => x.LastName, value);
			}
		}

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		public string Email
		{
			get
			{
				return this.pEmail;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pEmail, (UserDBEntity x) => x.Email, value);
			}
		}

		/// <summary>
		/// Gets or sets SecurityStamp.
		/// </summary>
		public string PasswordHash
		{
			get
			{
				return this.pPasswordHash;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pPasswordHash, (UserDBEntity x) => x.PasswordHash, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether EmailConfirmed.
		/// </summary>
		public bool EmailConfirmed
		{
			get
			{
				return this.pEmailConfirmed;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, bool>(ref this.pEmailConfirmed, (UserDBEntity x) => x.EmailConfirmed, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the phone number of the user has been confirmed.
		/// </summary>
		public bool PhoneNumberConfirmed
		{
			get
			{
				return this.pPhoneNumberConfirmed;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, bool>(ref this.pPhoneNumberConfirmed, (UserDBEntity x) => x.PhoneNumberConfirmed, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether TwoFactorEnabled.
		/// </summary>
		public bool TwoFactorEnabled
		{
			get
			{
				return this.pTwoFactorEnabled;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, bool>(ref this.pTwoFactorEnabled, (UserDBEntity x) => x.TwoFactorEnabled, value);
			}
		}

		/// <summary>
		/// Gets or sets AccessFailedCount.
		/// </summary>
		public int AccessFailedCount
		{
			get
			{
				return this.pAccessFailedCount;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, int>(ref this.pAccessFailedCount, (UserDBEntity x) => x.AccessFailedCount, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether LockoutEnabled.
		/// </summary>
		public bool LockoutEnabled
		{
			get
			{
				return this.pLockoutEnabled;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, bool>(ref this.pLockoutEnabled, (UserDBEntity x) => x.LockoutEnabled, value);
			}
		}

		/// <summary>
		/// Gets or sets UserName.
		/// </summary>
		public string UserName
		{
			get
			{
				return this.pUserName;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pUserName, (UserDBEntity x) => x.UserName, value);
			}
		}

		/// <summary>
		/// Gets or sets DateOfBirth.
		/// </summary>
		[DisplayName("Date Of Birth")]
		public DateTime? DateOfBirth
		{
			get
			{
				return this.pDateOfBirth;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, DateTime?>(ref this.pDateOfBirth, (UserDBEntity x) => x.DateOfBirth, value);
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
				return this.pAddress;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pAddress, (UserDBEntity x) => x.Address, value);
			}
		}

		/// <summary>
		/// Gets or sets City.
		/// </summary>
		public string City
		{
			get
			{
				return this.pCity;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pCity, (UserDBEntity x) => x.City, value);
			}
		}

		/// <summary>
		/// Gets or sets State.
		/// </summary>
		public string State
		{
			get
			{
				return this.pState;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pState, (UserDBEntity x) => x.State, value);
			}
		}

		/// <summary>
		/// Gets or sets Country.
		/// </summary>
		public string Country
		{
			get
			{
				return this.pCountry;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pCountry, (UserDBEntity x) => x.Country, value);
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
				return this.pPostalCode;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pPostalCode, (UserDBEntity x) => x.PostalCode, value);
			}
		}

		/// <summary>
		/// Gets or sets PhoneNumber.
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
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pPhoneNumber, (UserDBEntity x) => x.PhoneNumber, value);
			}
		}

		/// <summary>
		/// Gets or sets PhoneExtension.
		/// </summary>
		[DisplayName("Phone Extension")]
		public string PhoneExtension
		{
			get
			{
				return this.pPhoneExtension;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, string>(ref this.pPhoneExtension, (UserDBEntity x) => x.PhoneExtension, value);
			}
		}

		/// <summary>
		/// Gets or sets the last active subscription id.
		/// </summary>
		public int LastSubscriptionId
		{
			get
			{
				return this.pLastSubscriptionId;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, int>(ref this.pLastSubscriptionId, (UserDBEntity x) => x.LastSubscriptionId, value);
			}
		}

		/// <summary>
		/// Gets or sets the last active organization id.
		/// </summary>
		public int ActiveOrganizationId
		{
			get
			{
				return this.pActiveOrganizationId;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, int>(ref this.pActiveOrganizationId, (UserDBEntity x) => x.ActiveOrganizationId, value);
			}
		}

		/// <summary>
		/// Gets or sets the language preference.
		/// </summary>
		public int LanguagePreference
		{
			get
			{
				return this.pLanguagePreference;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, int>(ref this.pLanguagePreference, (UserDBEntity x) => x.LanguagePreference, value);
			}
		}

		/// <summary>
		/// Gets or sets LockoutEndDateUtc.
		/// </summary>
		public DateTime? LockoutEndDateUtc
		{
			get
			{
				return this.pLockoutEndDateUtc;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, DateTime?>(ref this.pLockoutEndDateUtc, (UserDBEntity x) => x.LockoutEndDateUtc, value);
			}
		}

		/// <summary>
		/// Gets or sets the password reset code.
		/// </summary>
		public Guid? PasswordResetCode
		{
			get
			{
				return this.passwordResetCode;
			}

			set
			{
				this.ApplyPropertyChange<UserDBEntity, Guid?>(ref this.passwordResetCode, (UserDBEntity x) => x.passwordResetCode, value);
			}
		}
	}
}