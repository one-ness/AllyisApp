//------------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all account related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		#region public static

		public static DateTime SetStartingDate(DateTime? date, int startOfWeek)
		{
			if (date == null && !date.HasValue)
			{
				DateTime today = DateTime.Now;
				int daysIntoTheWeek = (int)today.DayOfWeek < startOfWeek
					? (int)today.DayOfWeek + (7 - startOfWeek)
					: (int)today.DayOfWeek - startOfWeek;

				date = today.AddDays(-daysIntoTheWeek);
			}

			return date.Value.Date;
		}

		#endregion public static
	}
}