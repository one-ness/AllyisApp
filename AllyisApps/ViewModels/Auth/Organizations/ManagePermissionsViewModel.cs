using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Resources;
using AllyisApps.Services.Auth;
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
		public List<SubscriptionDisplayViewModel> Subscriptions { get; set; }

		/// <summary>
		/// Gets or sets a list of subscription ids for quick index reference.
		/// </summary>
		public List<int> SubIds { get; set; }

		// TODO: Eliminate these once product panes in Permissions page are genericized

		/// <summary>
		/// Gets or sets the product id for TimeTracker.
		/// </summary>
		public ProductIdEnum TimeTrackerId { get; set; }

		/// <summary>
		/// Gets or sets the product id for ExpenseTracker.
		/// </summary>
		public ProductIdEnum ExpenseTrackerId { get; set; }

		/// <summary>
		/// Gets or sets the index of TimeTracker in the subscriptions list.
		/// </summary>
		public int TimeTrackerSubIndex { get; set; }

		/// <summary>
		/// Gets or sets the index of ExpenseTracker in the subscriptions list.
		/// </summary>
		public int ExpenseTrackerSubIndex { get; set; }

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

	/// <summary>
	/// Subscription Permission vView s
	/// </summary>
	public class PermissionsViewModel
	{
		/// <summary>
		/// Users
		/// </summary>
		public List<UserPermssionViewModel> Users;

		/// <summary>
		/// Subscription
		/// </summary>
		public List<OrganizaionSubscriptionsViewModel> CurrentSubscriptions;

		public class OrganizaionSubscriptionsViewModel
		{
			/// <summary>
			///
			/// </summary>
			public int ProductId { get; set; }

			public int SubscriptionId { get; set; }

			public int SubscriptionName { get; set; }
		}
	}

	/// <summary>
	/// View Model for manage permssions for orgainzation
	/// </summary>
	public class OrganizaionPermissionsViewModel : PermissionsViewModel
	{
		public SelectList OrganizaionRoles { get; set; }
	}

	/// <summary>
	/// View Model for manage permssions for subscription
	/// </summary>
	public class SubscriptionPermissionsViewModel : PermissionsViewModel
	{
		/// <summary>
		/// Gets or sets available actions for the rows.
		/// </summary>
		public SelectList ProductRoles { get; set; }
	}

	/// <summary>
	/// Row in
	/// </summary>
	public class UserPermssionViewModel
	{
		/// <summary>
		/// UserId
		/// </summary>
		public int UserID { get; set; }

		/// <summary>
		///
		/// </summary>
		public ProductIdEnum productId { get; set; }

		/// <summary>
		/// Current product Role
		/// </summary>
		public int currentRole { get; set; }

		/// <summary>
		///
		/// </summary>
		public string currentRoleName { get; set; }

		/// <summary>
		/// User First Name
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// User Last Name
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Is selected for the action
		/// </summary>
		public bool isChecked { get; set; }
	}
}