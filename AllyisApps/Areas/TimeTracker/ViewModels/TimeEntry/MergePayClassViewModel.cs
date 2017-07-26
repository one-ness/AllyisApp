﻿//------------------------------------------------------------------------------
// <copyright file="MergePayClassViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.TimeTracker;
using System.Collections.Generic;

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
		public int sourcePayClassId { get; set; }

		/// <summary>
		/// Gets or sets the name of the pay class chosen to be merged.
		/// </summary>
		public string sourcePayClassName { get; set; }

		/// <summary>
		/// The subscription's Id
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the pay classes that can be merged into.
		/// </summary>
		public IEnumerable<PayClass> destinationPayClasses { get; set; }
	}
}
