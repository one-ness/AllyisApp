//------------------------------------------------------------------------------
// <copyright file="PayPeriodRanges.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace AllyisApps.Services.TimeTracker
{
	/// <summary>
	/// An object containing previous, current, and next pay period ranges
	/// </summary>
	public class PayPeriodRanges
	{
		/// <summary>
		/// Gets or sets the previous pay period range.
		/// This is the pay period range right before the current range.
		/// </summary>
		public DateRange Previous { get; set; }

		/// <summary>
		/// Gets or sets the current pay period range.
		/// This is the pay period range that contains the current date.
		/// </summary>
		public DateRange Current { get; set; }

		/// <summary>
		/// Gets or sets the next pay period range.
		/// This is the pay period range right after the current range.
		/// </summary>
		public DateRange Next { get; set; }
	}
}