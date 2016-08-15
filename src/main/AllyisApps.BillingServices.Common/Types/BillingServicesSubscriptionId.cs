//------------------------------------------------------------------------------
// <copyright file="BillingServicesSubscriptionId.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.BillingServices.Common.Types
{
	/// <summary>
	/// Container class for a Billing Services Subscription ID.
	/// </summary>
	public class BillingServicesSubscriptionId
	{
		#region private fields
		private readonly string id;
		#endregion

		#region constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="BillingServicesSubscriptionId"/> class.
		/// </summary>
		/// <param name="id">The ID for the container.</param>
		public BillingServicesSubscriptionId(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException("id", "id must have a value.");
			}

			this.id = id;
		}
		#endregion

		#region accessor properties
		/// <summary>
		/// Gets the subscription id.
		/// </summary>
		public string Id
		{
			get
			{
				return this.id;
			}
		}
		#endregion
	}
}
