//------------------------------------------------------------------------------
// <copyright file="BuiltinPayClassEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Hrm
{
	public enum BuiltinPayClassEnum : int
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
		Overtime = 5
	}
}