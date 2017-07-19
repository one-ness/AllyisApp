//------------------------------------------------------------------------------
// <copyright file="User.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// Represents a user.
	/// </summary>
	public class User
	{
		/// <summary>
		/// Gets or sets User Id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets First name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets Last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets Date of Birth.
		/// </summary>
		public DateTime? DateOfBirth { get; set; }

		/// <summary>
		/// Gets or sets Street address.
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
		/// Gets or sets Postal code.
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets Email address.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets Phone number.
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets Phone extension.
		/// </summary>
		public string PhoneExtension { get; set; }

		/// <summary>
		/// Gets or sets Id of the last subscription used/viewed.
		/// </summary>
		public int LastSubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the email address has been confirmed.
		/// </summary>
		public bool EmailConfirmed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the phone number has been confirmed.
		/// </summary>
		public bool PhoneNumberConfirmed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether two factor authentication has been enabled.
		/// </summary>
		public bool TwoFactorEnabled { get; set; }

		/// <summary>
		/// Gets or sets Number of access failures.
		/// </summary>
		public int AccessFailedCount { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether lockout is enabled.
		/// </summary>
		public bool LockoutEnabled { get; set; }

		/// <summary>
		/// Gets or sets Id of this user's active organization.
		/// </summary>
		public int ActiveOrganizationId { get; set; }

		/// <summary>
		/// Gets or sets End date for lockout.
		/// </summary>
		public DateTime? LockoutEndDateUtc { get; set; }

		/// <summary>
		/// Gets or sets Hash of this user's password.
		/// </summary>
		public string PasswordHash { get; set; }

		/// <summary>
		/// Gets or sets Code issued for password reset.
		/// </summary>
		public Guid? PasswordResetCode { get; set; }

		/// <summary>
		/// Gets or sets EmailConfirmationCode.
		/// </summary>
		public Guid? EmailConfirmationCode { get; set; }
	}
}
