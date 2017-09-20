//------------------------------------------------------------------------------
// <copyright file="OrganizationUserInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// Represents an organization user.
	/// </summary>
	public class Account
	{
		/// <summary>
		/// Gets or sets the organization's ID.
		/// </summary>
		public int AccountId { get; set; }

		/// <summary>
		/// Gets or sets the Account Name.
		/// </summary>
		public string AccountName { get; set; }

		/// <summary>
		/// Gets or sets whether the account is active.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets the AccountTypeId (TODO: what are the account types).
		/// </summary>
		public int AccountTypeId { get; set; }

		/// <summary>
		/// Gets or sets accounts parent account.
		/// </summary>
		public int? ParentAccountId { get; set; }

		/// <summary>
		/// Gets or sets the account type name, linked to AccountTypeId .
		/// </summary>
		public string AccountTypeName { get; set; }
	}
}