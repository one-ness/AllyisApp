//------------------------------------------------------------------------------
// <copyright file="BillingServicesCharge.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Common.Types
{
	/// <summary>
	/// An info object for a Billing Services Charge.
	/// </summary>
	public class BillingServicesCharge
	{
		#region private fields

		private readonly int amount;
		private readonly DateTime created;
		private readonly string id;
		private readonly string statementDescriptor;
		private readonly string service;

		#endregion private fields

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BillingServicesCharge"/> class.
		/// </summary>
		/// <param name="amount">The charge amount.</param>
		/// <param name="created">The date for the charge.</param>
		/// <param name="id">The charge Id.</param>
		/// <param name="statementDescriptor">Description text for the charge.</param>
		/// <param name="service">The billing service associated with the charge.</param>
		public BillingServicesCharge(int amount, DateTime created, string id, string statementDescriptor, string service = "Stripe")
		{
			this.amount = amount;
			this.created = created;
			this.id = id;
			this.statementDescriptor = statementDescriptor;
			this.service = service;
		}

		#endregion constructor

		#region accessor properties

		/// <summary>
		/// Gets the charge amount.
		/// </summary>
		public int Amount
		{
			get
			{
				return amount;
			}
		}

		/// <summary>
		/// Gets the charge date.
		/// </summary>
		public DateTime Created
		{
			get
			{
				return created;
			}
		}

		/// <summary>
		/// Gets the charge Id.
		/// </summary>
		public string Id
		{
			get
			{
				return id;
			}
		}

		/// <summary>
		/// Gets the description statement for the charge.
		/// </summary>
		public string StatementDescriptor
		{
			get
			{
				return statementDescriptor;
			}
		}

		/// <summary>
		/// Gets the service associated with the charge.
		/// </summary>
		public string Service
		{
			get
			{
				return service;
			}
		}

		#endregion accessor properties
	}
}