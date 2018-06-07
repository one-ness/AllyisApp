//------------------------------------------------------------------------------
// <copyright file="BuiltinPayClassEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace AllyisApps.Services.Hrm
{
	[CLSCompliant(false)]
	public enum BuiltinPayClassEnum : uint
	{
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
		UnpaidTimeOff = 4,

		/// <summary>
		/// Holiday.
		/// </summary>
		Holiday = 8,

		/// <summary>
		/// Overtime.
		/// </summary>
		Overtime = 16,

		/// <summary>
		/// Indentify customer created pay classes
		/// </summary>
		Custom = 32,

		/// <summary>
		/// any of these
		/// </summary>
		Any = uint.MaxValue,
	}
}
