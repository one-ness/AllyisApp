//------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for the Settings view.
	/// </summary>
	public class SettingsViewModel
	{
		/// <summary>
		/// Gets or sets the settings for an organization.
		/// </summary>
		public Setting Settings { get; set; }

		/// <summary>
		/// Gets or sets the pay classes for an organization.
		/// </summary>
		public IEnumerable<PayClass> PayClasses { get; set; }

		/// <summary>
		/// Gets or sets the holidays for an organization.
		/// </summary>
		public IEnumerable<Holiday> Holidays { get; set; }

		/// <summary>
		/// Subscription Id for the customer
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// User's Id
		/// </summary>
		public int UserId { get; set; }
	}
}
