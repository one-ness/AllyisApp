﻿//------------------------------------------------------------------------------
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
	public class UserDBEntity
	{
		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId { get; set; }

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
		/// Gets or sets SecurityStamp.
		/// </summary>
		public string PasswordHash { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether EmailConfirmed.
		/// </summary>
		public bool EmailConfirmed { get; set; }

		/// <summary>
		/// Gets or sets EmailConfirmationCode.
		/// </summary>
		public Guid? EmailConfirmationCode { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the phone number of the user has been confirmed.
		/// </summary>
		public bool PhoneNumberConfirmed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether TwoFactorEnabled.
		/// </summary>
		public bool TwoFactorEnabled { get; set; }

		/// <summary>
		/// Gets or sets AccessFailedCount.
		/// </summary>
		public int AccessFailedCount { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether LockoutEnabled.
		/// </summary>
		public bool LockoutEnabled { get; set; }

		/// <summary>
		/// Gets or sets DateOfBirth.
		/// </summary>
		public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets user's Address Id.
        /// </summary>
        public int AddressId { get; set; }

		/// <summary>
		/// Gets or sets Address.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets City.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets State.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Gets or sets Country.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets PostalCode.
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets PhoneNumber.
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets PhoneExtension.
		/// </summary>
		public string PhoneExtension { get; set; }

		/// <summary>
		/// Gets or sets the last active subscription id.
		/// </summary>
		public int? LastUsedSubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the last active organization id.
		/// </summary>
		public int? LastUsedOrganizationId { get; set; }

		/// <summary>
		/// Gets or sets LockoutEndDateUtc.
		/// </summary>
		public DateTime? LockoutEndDateUtc { get; set; }

		/// <summary>
		/// Gets or sets the password reset code.
		/// </summary>
		public Guid? PasswordResetCode { get; set; }

		/// <summary>
		/// Gets or sets the language preference.
		/// </summary>
		public int? PreferredLanguageId { get; set; }
	}
}
