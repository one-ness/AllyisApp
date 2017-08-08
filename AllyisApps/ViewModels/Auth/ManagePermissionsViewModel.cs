using System.Collections.Generic;
using AllyisApps.Services.Billing;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// The view model for the Edit Permissions page (Permission2).
	/// </summary>
	public class ManagePermissionsViewModel
	{
		/// <summary>
		/// Gets or sets the list of users and their permissions.
		/// </summary>
		public List<UserPermissionsViewModel> Users { get; set; }

		/// <summary>
		/// Gets or sets the list of subscriptions in the organization.
		/// </summary>
		public List<SubscriptionDisplayInfo> Subscriptions { get; set; }

		/// <summary>
		/// Gets or sets a list of subscription ids for quick index reference.
		/// </summary>
		public List<int> SubIds { get; set; }

		// TODO: Eliminate these once product panes in Permissions page are genericized
		/// <summary>
		/// Gets or sets the product id for TimeTracker.
		/// </summary>
		public int TimeTrackerId { get; set; }

		/// <summary>
		/// Gets or sets the index of TimeTracker in the subscriptions list.
		/// </summary>
		public int TimeTrackerSubIndex { get; set; }

		/// <summary>
		/// Gets or sets the Id of the organization associated with this manager.
		/// </summary>
		public int OrganizationId { get; set; }
	}

	/// <summary>
	/// A sub-view model for each user and their permissions in all subscriptions.
	/// </summary>
	public class UserPermissionsViewModel
	{
		/// <summary>
		/// Gets or sets the user's first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the user's last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the user's email.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the user's id.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets this user's role in the organization.
		/// </summary>
		public int OrganizationRoleId { get; set; }

		/// <summary>
		/// Gets or sets this user's role in each product. List order should be same as in list of subscriptions.
		/// </summary>
		public List<int> ProductRoleIds { get; set; }
	}
}
