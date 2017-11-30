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
		public DateRange() { }

		public DateRange(DateTime startDate, DateTime endDate)
		{
			StartDate = startDate;
			EndDate = endDate;
		}

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
