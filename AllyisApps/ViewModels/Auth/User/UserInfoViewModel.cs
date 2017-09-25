//------------------------------------------------------------------------------
// <copyright file="UserInfoViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents basic user info.
	/// </summary>
	public class UserInfoViewModel
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
		/// Gets or sets Address of loading User.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets Countryname of User.
		/// </summary>
		public string CountryName { get; set; }

		/// <summary>
		/// Gets or sets StateName of User.
		/// </summary>
		public string StateName { get; set; }

		/// <summary>
		/// Gets or sets City of User.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets Zip/Postalcode of User.
		/// </summary>
		public string PostalCode { get; set; }
	}
}