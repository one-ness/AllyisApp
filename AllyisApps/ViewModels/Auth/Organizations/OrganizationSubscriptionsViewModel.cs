using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// list of subscriptions of an organzation
	/// </summary>
	public class OrganizationSubscriptionsViewModel : BaseViewModel
	{
		/// <summary>
		/// Gets or sets a value indicating whether the user has permission to edit this subscription.
		/// </summary>
		public bool CanEditSubscriptions { get; set; }

		/// <summary>
		/// list of subscriptions
		/// </summary>
		public List<ViewModelItem> Subscriptions { get; set; }

		/// <summary>
		/// Gets or sets the organization's id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public OrganizationSubscriptionsViewModel()
		{
			this.Subscriptions = new List<ViewModelItem>();
		}

		/// <summary>
		/// subscription
		/// </summary>
		public class ViewModelItem
		{
			/// <summary>
			/// Gets or sets the name of the product this subscription is for.
			/// </summary>
			public string ProductName { get; set; }

			/// <summary>
			/// Gets or sets the product description.
			/// </summary>
			public string ProductDescription { get; set; }

			/// <summary>
			/// Gets or sets the subscriptionId.
			/// </summary>
			public int SubscriptionId { get; set; }

			/// <summary>
			/// Gets or sets the subscriptionName.
			/// </summary>
			public string SubscriptionName { get; set; }

			/// <summary>
			/// Gets or sets the product area url.
			/// </summary>
			public string AreaUrl { get; set; }

			/// <summary>
			/// Gets or sets Number of users in the subscription.
			/// </summary>
			public int NumberofUsers { get; set; }
		}
	}
}
