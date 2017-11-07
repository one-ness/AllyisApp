//------------------------------------------------------------------------------
// <copyright file="UserContext.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.Services.Billing;

namespace AllyisApps.Services.Auth
{
    /// <summary>
    /// Logged in user context.
    /// An instance must be created for every HTTP request to the application.
    /// This is also used as the cookie data.
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// Gets or sets the database id of the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the preferred language for this user.
        /// </summary>
        public string PreferedLanguageId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContext"/> class.
        /// </summary>
        public UserContext()
        {
            OrganizationsAndRoles = new Dictionary<int, OrganizationAndRole>();
            SubscriptionsAndRoles = new Dictionary<int, SubscriptionAndRole>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContext"/> class.
        /// </summary>
        public UserContext(int userId, string email, string firstName, string lastName, string preferredLanguageId = "en-US") : this()
        {
            if (userId <= 0) throw new ArgumentException("userId");
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("firstName");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("lastName");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email");

            Email = email;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PreferedLanguageId = preferredLanguageId;
        }

        /// <summary>
        /// list of oragnizations this user belongs to and the role in each organization, indexed by organziation id
        /// </summary>
        public Dictionary<int, OrganizationAndRole> OrganizationsAndRoles { get; set; }

        /// <summary>
        /// list of subscriptions this user belongs to and the role in each subscription, indexed by subscription id
        /// </summary>
        public Dictionary<int, SubscriptionAndRole> SubscriptionsAndRoles { get; set; }

        /// <summary>
        /// organization and role
        /// </summary>
        public class OrganizationAndRole
        {
            public int OrganizationId { get; set; }
            public OrganizationRoleEnum OrganizationRole { get; set; }
            public string OrganizationName { get; set; }
        }

        /// <summary>
        /// subscription and role information for the user
        /// </summary>
        public class SubscriptionAndRole
        {
            public int SubscriptionId { get; set; }
            public int OrganizationId { get; set; }
            public SkuIdEnum SkuId { get; set; }
            public ProductIdEnum ProductId { get; set; }
            public int ProductRoleId { get; set; }
            public string AreaUrl { get; set; }
			public string SubscriptionName { get; internal set; }
		}
    }
}