//------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
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
		public SettingsInfoViewModel Settings { get; set; }

		/// <summary>
		/// Gets or sets the pay classes for an organization.
		/// </summary>
		public IEnumerable<PayClassViewModel> PayClasses { get; set; }

		/// <summary>
		/// Gets or sets the holidays for an organization.
		/// </summary>
		public IEnumerable<HolidayViewModel> Holidays { get; set; }

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
		/// Settings View Model.
		/// </summary>
		public class SettingsInfoViewModel
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

			/// <summary>
			/// Gets or sets a value indicating whether to use a lock date.
			/// </summary>
			public bool IsLockDateUsed { get; set; }

			/// <summary>
			/// Gets or sets the lock date period (days/weeks/months).
			/// </summary>
			public int LockDatePeriod { get; set; }

			/// <summary>
			/// Gets or sets the lock date quantity.
			/// </summary>
			public int LockDateQuantity { get; set; }

			/// <summary>
			/// Get or sets todays date for holiday datepicker
			/// </summary>
			public DateTime Today { get; set; }
		}

		/// <summary>
		/// Pay classes View for settings page.
		/// </summary>
		public class PayClassViewModel
		{
			/// <summary>
			/// Gets or sets Name of pay Class.
			/// </summary>
			public string PayClassName { get; set; }

			/// <summary>
			/// Gets or sets id of pay Class.
			/// </summary>
			public int PayClassId { get; set; }
		}

		/// <summary>
		/// Holiday View Model for settings page.
		/// </summary>
		public class HolidayViewModel
		{
			/// <summary>
			/// Gets or sets Holiday Date.
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// Gets or setsHoliday Name.
			/// </summary>
			public string HolidayName { get; set; }

			/// <summary>
			/// Gets or sets Holiday Id.
			/// </summary>
			public int HolidayId { get; set; }
		}
	}
}