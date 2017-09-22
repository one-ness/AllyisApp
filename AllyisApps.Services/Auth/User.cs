//------------------------------------------------------------------------------
// <copyright file="User.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.Lookup;
using System;
using System.Collections.Generic;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// Represents a user.
	/// </summary>
	public class User
	{
		public const int PasswordMinLength = 8;

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
		/// Gets or sets the User's address.
		/// </summary>
		public Address Address { get; set; }

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
		public int? LastUsedSubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the email address has been confirmed.
		/// </summary>
		public bool IsEmailConfirmed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the phone number has been confirmed.
		/// </summary>
		public bool IsPhoneNumberConfirmed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether two factor authentication has been enabled.
		/// </summary>
		public bool IsTwoFactorEnabled { get; set; }

		/// <summary>
		/// Gets or sets Number of access failures.
		/// </summary>
		public int AccessFailedCount { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether lockout is enabled.
		/// </summary>
		public bool IsLockoutEnabled { get; set; }

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

		/// <summary>
		/// Gets or sets the created time
		/// </summary>
		public DateTime UserCreatedUtc { get; set; }

		/// <summary>
		/// list of organizations the user is member of
		/// </summary>
		public List<UserOrganization> Organizations { get; set; }

		/// <summary>
		/// list of subscriptions the user is a member of
		/// </summary>
		public List<UserSubscription> Subscriptions { get; set; }

		/// <summary>
		/// list of invitations pending for the user
		/// </summary>
		public List<Invitation> Invitations { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public User()
		{
			this.Address = new Address();
			this.Organizations = new List<UserOrganization>();
			this.Subscriptions = new List<UserSubscription>();
			this.Invitations = new List<Invitation>();
		}
	}
}