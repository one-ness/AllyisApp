//------------------------------------------------------------------------------
// <copyright file="CustomerInfoViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;

namespace AllyisApps.ViewModels.TimeTracker.Customer
{
	/// <summary>
	/// Represents basic Customer info.
	/// </summary>
	public class CustomerInfoViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the name of the organization that the Customer belongs to.
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets the Customer's info object.
		/// </summary>
		public CustomerInfo CustomerInfo { get; internal set; }
	}
}