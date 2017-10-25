//------------------------------------------------------------------------------
// <copyright file="BillingRateEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace AllyisApps.Services
{
	/// <summary>
	/// Rates of pay for positions.
	/// </summary>
	public enum BillingRateEnum
	{
		/// <summary>
		/// Hourly Pay Rate.
		/// </summary>
		[Display(Name = "Hourly")]
		Hourly = 201,

		/// <summary>
		/// Pay rate by Month.
		/// </summary>
		[Display(Name = "Monthly")]
		Monthly = 301,

		/// <summary>
		/// Pay rate by Year.
		/// </summary>
		[Display(Name = "Yearly")]
		Yearly = 401,
	}
}