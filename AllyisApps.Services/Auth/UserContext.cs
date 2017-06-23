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
			this.UserSubscriptions = new Dictionary<int, UserSubscription>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserContext"/> class.
		/// </summary>
		/// <param name="userId">The user ID.</param>
        /// <param name="firstName">The user's first name</param>
        /// <param name="lastName">The user's last name</param>
		/// <param name="username">The username.</param>
		/// <param name="email">The email.</param>
		/// <param name="chosenOrganizationId">The chosen Organization ID.</param>
		/// <param name="chosenSubscriptionId">The chosen subscription ID.</param>
		/// <param name="chosenLanguageID">The chosen language ID.</param>
		public UserContext(int userId, string username, string email, string firstName = "First", string lastName = "Last", int chosenOrganizationId = 0, int chosenSubscriptionId = 0, int chosenLanguageID = 0) : this()
		{
			if (userId <= 0) throw new ArgumentException("userId");
			if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("username");
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email");

			this.Email = email;
			this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
			this.UserName = username;
			this.ChosenOrganizationId = chosenOrganizationId;
			this.ChosenSubscriptionId = chosenSubscriptionId;
			this.ChosenLanguageId = chosenLanguageID;
		}

		/// <summary>
		/// Gets or sets the database id of the user.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		[JsonIgnore]
		public string UserName { get; set; }

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
		/// Gets or sets the organization id the user chooses to work on (or last worked on).
		/// </summary>
		[JsonIgnore]
		public int ChosenOrganizationId { get; set; }

		/// <summary>
		/// Gets the chosen organization
		/// </summary>
		public UserOrganization ChosenOrganization
		{
			get
			{
				UserOrganization result = null;
				this.UserOrganizations.TryGetValue(this.ChosenOrganizationId, out result);
				return result;
			}
		}

		/// <summary>
		/// Gets or sets the subscription id the user chooses to work on (or last worked on).
		/// </summary>
		[JsonIgnore]
		public int ChosenSubscriptionId { get; set; }

		/// <summary>
		/// Gets the chosen subscription
		/// </summary>
		public UserSubscription ChosenSubscription
		{
			get
			{
				UserSubscription result = null;
				this.UserSubscriptions.TryGetValue(this.ChosenSubscriptionId, out result);
				return result;
			}
		}

		/// <summary>
		/// Gets or sets the list of organizations the user is a member of, role in the organization, subscriptions each organization has subscribed to and user's role in that subscription.
		/// </summary>
		[JsonIgnore]
		public Dictionary<int, UserOrganization> UserOrganizations { get; set; }

		/// <summary>
		/// Gets or sets the list of subscriptions the user is a member of. This is essentially a flattened out list of each of the organization above.
		/// </summary>
		[JsonIgnore]
		public Dictionary<int, UserSubscription> UserSubscriptions { get; set; }

		/// <summary>
		/// Gets or sets the preferred language for this user.
		/// </summary>
		[JsonIgnore]
		public int ChosenLanguageId { get; set; }
	}
}
