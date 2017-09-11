//------------------------------------------------------------------------------
// <copyright file="CustomerInfoViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
		public Services.Customer CustomerInfo { get; internal set; }
	}
}