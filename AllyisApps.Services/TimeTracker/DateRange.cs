//------------------------------------------------------------------------------
// <copyright file="DateRange.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.TimeTracker
{
	/// <summary>
	/// An object that depicts a range of time, containing a start date and end date.
	/// </summary>
	public class DateRange
	{
		private DateTime startDate;
		private DateTime endDate;

		/// <summary>
		/// Creates a range where start date is less than end date.  Only available constructor for validation of proper range.
		/// </summary>
		/// <param name="startDate">Start of range</param>
		/// <param name="endDate">End of range</param>
		public DateRange(DateTime startDate, DateTime endDate)
		{
			if (startDate > endDate)
			{
				throw new ArgumentOutOfRangeException(nameof(StartDate), startDate, "Start date cannot be greater than end date");
			}

			this.startDate = startDate;
			this.endDate = endDate;
		}

		public DateTime StartDate
		{
			get => startDate;
			set
			{
				if (value > endDate)
				{
					throw new ArgumentOutOfRangeException(nameof(StartDate), value, "Start date cannot be greater than end date");
				}

				startDate = value;
			}
		}

		public DateTime EndDate
		{
			get => endDate;
			set
			{
				if (value < startDate)
				{
					throw new ArgumentOutOfRangeException(nameof(EndDate), value, "End date cannot be less than start date");
				}

				endDate = value;
			}
		}
	}
}
