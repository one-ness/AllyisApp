//------------------------------------------------------------------------------
// <copyright file="OrganizationUser.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// Represents an organization user.
	/// </summary>
	public class OrganizationUser : User
	{
		/// <summary>
		/// Gets or sets the Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Organization role Id.
		/// </summary>
		public int OrganizationRoleId { get; set; }

		/// <summary>
		/// Gets or sets the Date this user was added to the organization.
		/// </summary>
		public DateTime OrganizationUserCreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// approval limit for this user in expense tracker
		/// </summary>
		public decimal ExpenseApprovalLimit { get; set; }

		/// <summary>
		/// Gets or set the employee type id
		/// </summary>
		public int EmployeeTypeId { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public OrganizationUser() { }

		/// <summary>
		/// constructor
		/// </summary>
		public OrganizationUser(User user)
		{
			this.AccessFailedCount = user.AccessFailedCount;
			this.Address = user.Address;
			this.DateOfBirth = user.DateOfBirth;
			this.Email = user.Email;
			this.EmailConfirmationCode = user.EmailConfirmationCode;
			this.FirstName = user.FirstName;
			this.IsAddressLoaded = user.IsAddressLoaded;
			this.IsEmailConfirmed = user.IsEmailConfirmed;
			this.IsLockoutEnabled = user.IsLockoutEnabled;
			this.IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed;
			this.IsTwoFactorEnabled = user.IsTwoFactorEnabled;
			this.LastName = user.LastName;
			this.LastUsedSubscriptionId = user.LastUsedSubscriptionId;
			this.LockoutEndDateUtc = user.LockoutEndDateUtc;
			this.LoginProvider = user.LoginProvider;
			this.PasswordHash = user.PasswordHash;
			this.PasswordResetCode = user.PasswordResetCode;
			this.PhoneExtension = user.PhoneExtension;
			this.PhoneNumber = user.PhoneNumber;
			this.UserCreatedUtc = user.UserCreatedUtc;
			this.UserId = user.UserId;
		}
	}
}
