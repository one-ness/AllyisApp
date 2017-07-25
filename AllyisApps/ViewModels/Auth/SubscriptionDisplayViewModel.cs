//------------------------------------------------------------------------------
// <copyright file="SubscriptionDisplayViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services.Billing;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// View Model for display of subscription info on the organization manage page.
	/// </summary>
	public class SubscriptionDisplayViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets the subscription info object.
		/// </summary>
		public SubscriptionDisplayInfo Info { get; set; } // TODO: Move this information into view model directly?

		/// <summary>
		/// Gets or sets a value indicating whether the user has permission to edit this subscription.
		/// </summary>
		public bool CanEditSubscriptions { get; set; }

		/// <summary>
		/// Gets or sets the id for the product this subscription is for.
		/// Note: this is needed even though it also exists in SubscriptionDisplayInfo. Sometimes that is null.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// Gets or sets the name of the product this subscription is for.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the product description.
		/// </summary>
		public string ProductDescription { get; set; }

		/// <summary>
		/// Gets or sets the subscriptionId
		/// </summary>
		public int SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the subscriptionName
        /// </summary>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Gets or sets the organization's id
        /// </summary>
        public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the product area url
		/// </summary>
		public string AreaUrl { get; set; }
	}
}
