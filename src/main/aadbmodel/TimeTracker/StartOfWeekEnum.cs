﻿//------------------------------------------------------------------------------
// <copyright file="StartOfWeekEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.TimeTracker
{
	/// <summary>
	/// An enum to keep track of the starting day selection.
	/// </summary>
	public enum StartOfWeekEnum : int
	{
		/// <summary>
		/// Start: Sunday.
		/// </summary>
		Sunday,

		/// <summary>
		/// Start: Monday.
		/// </summary>
		Monday,

		/// <summary>
		/// Start: Tuesday.
		/// </summary>
		Tuesday,

		/// <summary>
		/// Start: Wednesday.
		/// </summary>
		Wednesday,

		/// <summary>
		/// Start: Thursday.
		/// </summary>
		Thursday,

		/// <summary>
		/// Start: Friday.
		/// </summary>
		Friday,

		/// <summary>
		/// Start: Saturday.
		/// </summary>
		Saturday
	}
}
