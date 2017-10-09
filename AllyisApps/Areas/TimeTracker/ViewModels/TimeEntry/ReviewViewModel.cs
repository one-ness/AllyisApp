//------------------------------------------------------------------------------
// <copyright file="ReviewTimeEntriesViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// 
	/// </summary>
	public class ReviewViewModel
	{
		/// <summary>
		/// Gets or sets the UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the SubscriptionId.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the SubscriptionName.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the list of pay classes that the organization has
		/// </summary>
		public IEnumerable<PayClassInfoViewModel> PayClasses { get; set; }
	}
}