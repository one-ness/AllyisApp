//------------------------------------------------------------------------------
// <copyright file="BillingServicesInvoice.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.BillingServices.Common.Types
{
	/// <summary>
	/// An info object for a Billing services Invoice.
	/// </summary>
	public class BillingServicesInvoice
	{
		#region private fields

		private readonly int amountDue;
		private readonly DateTime? date;
		private readonly string id;
		private readonly string productName;
		private readonly string service;

		#endregion private fields

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="BillingServicesInvoice"/> class.
		/// </summary>
		/// <param name="amountDue">The ammount due.</param>
		/// <param name="date">The invoice date.</param>
		/// <param name="id">The invoice id.</param>
		/// <param name="productName">The product name.</param>
		/// <param name="service">The service handling the invoice.</param>
		public BillingServicesInvoice(int amountDue, DateTime? date, string id, string productName, string service)
		{
			this.amountDue = amountDue;
			this.date = date;
			this.id = id;
			this.productName = productName;
			this.service = service;
		}

		#endregion constructor

		#region accessor properties

		/// <summary>
		/// Gets the amount due.
		/// </summary>
		public int AmountDue
		{
			get
			{
				return this.amountDue;
			}
		}

		/// <summary>
		/// Gets the date. Nullable.
		/// </summary>
		public DateTime? Date
		{
			get
			{
				return this.date;
			}
		}

		/// <summary>
		/// Gets the invoice id.
		/// </summary>
		public string Id
		{
			get
			{
				return this.id;
			}
		}

		/// <summary>
		/// Gets the product name.
		/// </summary>
		public string ProductName
		{
			get
			{
				return this.productName;
			}
		}

		/// <summary>
		/// Gets the billing service assoiated with the invoice.
		/// </summary>
		public string Service
		{
			get
			{
				return this.service;
			}
		}

		#endregion accessor properties
	}
}