//------------------------------------------------------------------------------
// <copyright file="BillingServicesCustomerId.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Common.Types
{
	/// <summary>
	/// Container class for a Billing Services Customer Id.
	/// </summary>
	public class BillingServicesCustomerId
	{
		#region private fields

		private readonly string id;

		#endregion private fields

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BillingServicesCustomerId"/> class.
		/// </summary>
		/// <param name="id">The Id for the container.</param>
		public BillingServicesCustomerId(string id)
		{
			this.id = id;
		}

		#endregion constructor

		#region accessor properties

		/// <summary>
		/// Gets the customer Id.
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
