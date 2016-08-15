//------------------------------------------------------------------------------
// <copyright file="SubscriptionUserInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// An object for keeping track of all the info related to a given subscription user.
	/// </summary>
	public class SubscriptionUserInfo
    {
        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets LastName.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets ProductRoleId.
        /// </summary>
        public string ProductRoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the subscribed product.
        /// </summary>
        public string ProductRoleName { get; set; }

        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the date the subscription user was added. 
        /// </summary>
        public DateTime CreatedUTC { get; set; }

        /// <summary>
        /// Gets or sets the id of the subscription.
        /// </summary>
        public int SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the SKU of the subscription.
        /// </summary>
        public int SkuId { get; set; }
    }
}
