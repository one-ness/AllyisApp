//------------------------------------------------------------------------------
// <copyright file="SkusListViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using AllyisApps.Services;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents the Skus List view.
	/// </summary>
	public class SkusListViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the collection of all products offered to user.
		/// </summary>
		public IEnumerable<Product> ProductsList { get; set; }

		/// <summary>
		/// Gets or sets the current Organization's Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the current Organization's active subscriptions.
		/// </summary>
		public IEnumerable<Subscription> CurrentSubscriptions { get; set; }
	}
}