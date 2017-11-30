﻿namespace AllyisApps.Services.TimeTracker
{
	public enum PayClassId
	{
		/// <summary>
		/// Indentify custome created ids
		/// </summary>
		Custom = 0, 
		/// <summary>
		/// Regular.
		/// </summary>
		Regular = 1,

		/// <summary>
		/// Paid Time Off.
		/// </summary>
		PaidTimeOff = 2,

		/// <summary>
		/// Unpaid Time Off.
		/// </summary>
		UnpaidTimeOff = 3,

		/// <summary>
		/// Holiday.
		/// </summary>
		Holiday = 4,

		/// <summary>
		/// Overtime.
		/// </summary>
		OverTime = 5,

		BeahvmentLeave = 6,

		OtherLeave =7,
		
	}
}