//------------------------------------------------------------------------------
// <copyright file="PayPeriodRanges.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.TimeTracker
{
	/// <summary>
	/// An object containing previous, current, and next pay period ranges
	/// </summary>
	public class PayPeriodRanges
	{
		/// <summary>
		/// An object that depicts a range of time, containing a start date and end date.
		/// </summary>
		public class PayPeriodRange
		{
			public PayPeriodRange() { }

			public PayPeriodRange(DateTime startDate, DateTime endDate)
			{
				StartDate = startDate;
				EndDate = endDate;
			}

			public DateTime StartDate { get; set; }
			public DateTime EndDate { get; set; }
		}

		public PayPeriodRanges()
		{
			Previous = new PayPeriodRange();
			Current = new PayPeriodRange();
			Next = new PayPeriodRange();
		}

		/// <summary>
		/// Gets or sets the previous pay period range.
		/// This is the pay period range right before the current range.
		/// </summary>
		public PayPeriodRange Previous { get; set; }

		/// <summary>
		/// Gets or sets the current pay period range.
		/// This is the pay period range that contains the current date.
		/// </summary>
		public PayPeriodRange Current { get; set; }

		/// <summary>
		/// Gets or sets the next pay period range.
		/// This is the pay period range right after the current range.
		/// </summary>
		public PayPeriodRange Next { get; set; }
	}
}