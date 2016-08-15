﻿//------------------------------------------------------------------------------
// <copyright file="BillingServicesCustomer.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.BillingServices.Common.Types
{
	/// <summary>
	/// An info object for a Billing Services Customer.
	/// </summary>
	public class BillingServicesCustomer
	{
		#region private fields
		private readonly BillingServicesCustomerId billingServicesCustomerId;
		private readonly string last4;
		private readonly string email;
		#endregion

		#region constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="BillingServicesCustomer"/> class.
		/// </summary>
		/// <param name="customerId">The billing services customer ID.</param>
		/// <param name="last4">The last 4 digits of the customer's payment card.</param>
		public BillingServicesCustomer(BillingServicesCustomerId customerId,string email, string last4 = "nnnn")
		{
			#region last4 validation
			if (last4.Length != 4)
			{
				throw new ArgumentException("last4", "last 4 must be exactly 4 numbers");
			}

			int num;
			if (last4 != "nnnn" && !int.TryParse(last4, out num))
			{
				throw new ArgumentException("last4", "last 4 must be exactly 4 numbers");
			}
			#endregion

			this.last4 = last4;
			this.billingServicesCustomerId = customerId;
			this.email = email;
		}
		#endregion

		#region accessor properties
		/// <summary>
		/// Gets the ID.
		/// </summary>
		public BillingServicesCustomerId Id
		{
			get
			{
				return this.billingServicesCustomerId;
			}
		}

		/// <summary>
		/// Gets the Last 4.
		/// </summary>
		public string Last4
		{
			get
			{
				return this.last4;
			}
		}

		/// <summary>
		/// Gets email.
		/// </summary>
		public string Email
		{
			get
			{
				return this.email;
			}
		}
		#endregion
	}
}
