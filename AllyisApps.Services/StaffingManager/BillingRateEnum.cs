//------------------------------------------------------------------------------
// <copyright file="BillingRateEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services
{
	/// <summary>
	/// Rates of pay for positions.
	/// </summary>
	public enum BillingRateEnum : int
	{
		/// <summary>
		/// Hourly Pay Rate.
		/// </summary>
		Hourly = 201,

		/// <summary>
		/// Pay rate by Month.
		/// </summary>
		Monthly = 301,

		/// <summary>
		/// Pay rate by Year.
		/// </summary>
		Yearly = 401,
	}
}