//------------------------------------------------------------------------------
// <copyright file="SettingsPayPeriodViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

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
		[Range(1, 365, ErrorMessage = "Enter a number between 1 and 365")]
		[RegularExpression("([1-9][0-9]{0,3})", ErrorMessage = "Enter a number between 1 and 365")]
		public int? Duration { get; set; }

		/// <summary>
		/// Gets or sets the dates value for pay period.  Comma deliniated list of dates
		/// </summary>
		[RegularExpression("([1-9][0-9]?)(, ?[1-9][0-9]?){0,15}", ErrorMessage = "Please enter a comma deliniated list of days, between 1-28")]
		public string Dates { get; set; }

		/// <summary>
		/// Gets or sets the pay period type.  0 is duration, 1 is dates
		/// </summary>
		public int PayPeriodTypeId { get; set; }


		/// <summary>
		/// Gets or sets the start date for the duration type pay period.
		/// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets the organization id that the view belongs to.
		/// </summary>
		public int OrganizationId { get; set; }
	}
}