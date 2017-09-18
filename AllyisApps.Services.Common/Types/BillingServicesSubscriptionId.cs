//------------------------------------------------------------------------------
// <copyright file="BillingServicesSubscriptionId.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Common.Types
{
	/// <summary>
	/// Container class for a Billing Services Subscription Id.
	/// </summary>
	public class BillingServicesSubscriptionId
	{
		#region private fields

		private readonly string id;

		#endregion private fields

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BillingServicesSubscriptionId"/> class.
		/// </summary>
		/// <param name="id">The Id for the container.</param>
		public BillingServicesSubscriptionId(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException("id", "id must have a value.");
			}

			this.id = id;
		}

		#endregion constructor

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

		#endregion accessor properties
	}
}