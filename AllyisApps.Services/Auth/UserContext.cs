//------------------------------------------------------------------------------
// <copyright file="UserContext.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AllyisApps.Services
{
	/// <summary>
	/// Logged in user context.
	/// An instance must be created for every HTTP request to the application.
	/// This is also used as the cookie data.
	/// </summary>
	public class UserContext
	{
		// TODO: Add project information somewhere here (in OrganizationInfo?)

		/// <summary>
		/// Initializes a new instance of the <see cref="UserContext"/> class.
		/// </summary>
		public UserContext()
		{
			this.UserOrganizations = new Dictionary<int, UserOrganization>();
			this.OrganizationSubscriptions = new Dictionary<int, UserSubscription>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserContext"/> class.
		/// </summary>
		public UserContext(int userId, string email, string firstName, string lastName, int preferredLanguageId = 0) : this()
		{
			if (userId <= 0) throw new ArgumentException("userId");
			if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("firstName");
			if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("lastName");
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email");

			this.Email = email;
			this.UserId = userId;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.PrefferedLanguageId = preferredLanguageId;
		}

		/// <summary>
		/// Gets or sets the database id of the user.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// First name of the user
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Last name of the user
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the email of the user.
		/// </summary>
		[JsonIgnore]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the list of organizations the user is a member of, role in the organization, subscriptions each organization has subscribed to and user's role in that subscription.
		/// </summary>
		[JsonIgnore]
		public Dictionary<int, UserOrganization> UserOrganizations { get; set; }

		/// <summary>
		/// Gets or sets the list of subscriptions this organization has.
		/// If the user is not a member of that subscrption, then the role is set to NotAssigned
		/// </summary>
		[JsonIgnore]
		public Dictionary<int, UserSubscription> OrganizationSubscriptions { get; set; }

		/// <summary>
		/// Gets or sets the preferred language for this user.
		/// </summary>
		[JsonIgnore]
		public int PrefferedLanguageId { get; set; }
	}
}
