//------------------------------------------------------------------------------
// <copyright file="SettingsPayPeriodViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for the Settings view.
	/// </summary>
	public class SettingsPayPeriodViewModel
	{
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

		/// <summary>
		/// Gets or sets the duration value for payperiod
		/// </summary>
		public int Duration { get; set; }

		/// <summary>
		/// Gets or sets the dates value for pay period
		/// </summary>
		public List<int> Dates { get; set; }

		/// <summary>
		/// Gets or sets the pay period type.  0 is duration, 1 is dates
		/// </summary>
		public int PayPeriodTypeId { get; set; }
	}
}