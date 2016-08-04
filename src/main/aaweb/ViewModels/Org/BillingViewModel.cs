//------------------------------------------------------------------------------
// <copyright file="BillingViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>

//------------------------------------------------------------------------------
using System;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// The billing model.
	/// </summary>
	[CLSCompliant(false)]
	public class BillingViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the billing email address. 
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the billing description.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the amount being billed.
		/// </summary>
		public int Amount { get; set; }

		/// <summary>
		/// Gets or sets the type of currency being paid. 
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// Gets or sets the customer to get billed. 
		/// </summary>
		[CLSCompliant(false)]
		public StripeCustomer Customer { get; set; }
	}
}