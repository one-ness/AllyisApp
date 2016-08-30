﻿//------------------------------------------------------------------------------
// <copyright file="SettingsInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// An object for keeping track of the settings for a time tracker.
	/// </summary>
	public class SettingsInfo
	{
		/// <summary>
		/// Gets or sets the OrganizationId.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the StartOfWeek.
		/// </summary>
		public int StartOfWeek { get; set; }

		/// <summary>
		/// Gets or sets the OvertimeHours.
		/// </summary>
		public int OvertimeHours { get; set; }

		/// <summary>
		/// Gets or sets the OvertimePeriod.
		/// </summary>
		public string OvertimePeriod { get; set; }

		/// <summary>
		/// Gets or sets the OvertimeMultiplier.
		/// </summary>
		public decimal OvertimeMultiplier { get; set; }
	}
}