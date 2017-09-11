//------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;

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
		/// Gets or sets subscription Id for the customer.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the Subscription Name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the user's Id.
		/// </summary>
		public int UserId { get; set; }
	}
}