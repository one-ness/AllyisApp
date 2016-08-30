﻿//------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.BusinessObjects;
using AllyisApps.Services.TimeTracker;

using System.Collections.Generic;

namespace AllyisApps.Areas.TimeTracker.Models
{
	/// <summary>
	/// Model for the Settings view.
	/// </summary>
	public class SettingsViewModel
	{
		/// <summary>
		/// Gets the start of week for an organization.
		/// </summary>
		public StartOfWeekEnum StartOfWeek { get; internal set; }

		/// <summary>
		/// Gets or sets the settings for an organization.
		/// </summary>
		public SettingsInfo Settings { get; set; }

		/// <summary>
		/// Gets or sets the pay classes for an organization.
		/// </summary>
		public IEnumerable<PayClassInfo> PayClasses { get; set; }

		/// <summary>
		/// Gets or sets the holidays for an organization.
		/// </summary>
		public IEnumerable<HolidayInfo> Holidays { get; set; }
	}
}