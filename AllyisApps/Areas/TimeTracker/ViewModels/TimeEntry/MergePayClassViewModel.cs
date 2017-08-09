//------------------------------------------------------------------------------
// <copyright file="MergePayClassViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.ViewModels.TimeTracker.TimeEntry
{
	/// <summary>
	/// Model for the MergePayClass view.
	/// </summary>
	public class MergePayClassViewModel
	{
		/// <summary>
		/// Gets or sets the id of the pay class chosen to be merged.
		/// </summary>
		public int SourcePayClassId { get; set; }

		/// <summary>
		/// Gets or sets the name of the pay class chosen to be merged.
		/// </summary>
		public string SourcePayClassName { get; set; }

		/// <summary>
		/// Gets or sets the subscription's Id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the pay classes that can be merged into.
		/// </summary>
		public IEnumerable<PayClass> DestinationPayClasses { get; set; }
	}
}
