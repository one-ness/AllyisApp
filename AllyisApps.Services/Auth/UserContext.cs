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
			this.UserOrganizationInfoList = new List<UserOrganizationInfo>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UserContext"/> class.
		/// </summary>
		/// <param name="userId">The user ID.</param>
		/// <param name="username">The username.</param>
		/// <param name="email">The email.</param>
		/// <param name="chosenOrganizationId">The chosen Organization ID.</param>
		/// <param name="chosenSubscriptionId">The chosen subscription ID.</param>
		/// <param name="infoList">The organization info list.</param>
		/// <param name="chosenLanguageID">The chosen language ID.</param>
		public UserContext(
			int userId,
			string username,
			string email,
			int chosenOrganizationId = 0,
			int chosenSubscriptionId = 0,
			List<UserOrganizationInfo> infoList = null,
			int chosenLanguageID = 0)
		{
			if (userId < 1)
			{
				throw new ArgumentOutOfRangeException("userId");
			}

			// if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username");
			// if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email");

			this.Email = email;
			this.UserId = userId;
			this.UserName = username;
			this.ChosenOrganizationId = chosenOrganizationId;
			this.ChosenSubscriptionId = chosenSubscriptionId;
			if (infoList == null)
			{
				this.UserOrganizationInfoList = new List<UserOrganizationInfo>();
			}
			else
			{
				this.UserOrganizationInfoList = infoList;
			}

			this.ChosenLanguageID = chosenLanguageID;
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
		/// Gets or sets the email of the user.
		/// </summary>
		[JsonIgnore]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the organization id the user chooses to work on (or last worked on).
		/// </summary>
		[JsonIgnore]
		public int ChosenOrganizationId { get; set; } // TODO: Decide whether to add logic to this and ChosenSubscriptionId's get and set methods to grab from database and update database

		/// <summary>
		/// Gets or sets the subscription id the user chooses to work on (or last worked on).
		/// </summary>
		[JsonIgnore]
		public int ChosenSubscriptionId { get; set; } // TODO: Decide whether to add logic to this and ChosenOrganizationId's get and set methods to grab from database and update database

		/// <summary>
		/// Gets or sets the list of organizations the user is member of, role in the organization, subscriptions each organization has subscribed to and user's role in that subscription.
		/// </summary>
		[JsonIgnore]
		public List<UserOrganizationInfo> UserOrganizationInfoList { get; set; }

		/// <summary>
		/// Gets or sets the preferred language for this user.
		/// </summary>
		[JsonIgnore]
		public int ChosenLanguageID { get; set; }
	}
}
