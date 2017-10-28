using System.Collections.Generic;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Subscription Permission vView s
	/// </summary>
	public class PermissionsViewModel
	{
		/// <summary>
		/// Group label for action list
		/// </summary>
		public string ActionGroup { get; set; }

		/// <summary>
		/// Actions Role Choices
		/// </summary>
		public Dictionary<string, int> Actions { get; set; }

		/// <summary>
		/// Available roles to be printed on table
		/// </summary>
		public Dictionary<int, string> PossibleRoles { get; set; }

		/// <summary>
		/// Organizaion Id
		/// </summary>
		public int OrganizationId;

		/// <summary>
		/// Subscription Id for is null if on OrgManagePage
		/// </summary>
		public int? SubscriptionId;

		/// <summary>
		/// ProductID for subscription is null if on OrgmanagePage
		/// </summary>
		public int? ProductId;

		/// <summary>
		/// Header text for Role
		/// </summary>
		public string RoleHeader { get; set; }

		/// <summary>
		/// Gets Message used for remove
		/// </summary>
		public string RemoveUserMessage { get; set; }

		/// <summary>
		/// Users
		/// </summary>
		public List<UserPermssionViewModel> Users;

		/// <summary>
		/// Subscription
		/// </summary>
		public List<OrganizaionSubscriptionsViewModel> CurrentSubscriptions;

		/// <summary>
		/// Organization Subscriptions
		/// </summary>
		public class OrganizaionSubscriptionsViewModel
		{
			/// <summary>
			///
			/// </summary>
			public int ProductId { get; set; }

			/// <summary>
			/// SubscripitonId for subscription
			/// </summary>
			public int SubscriptionId { get; set; }

			/// <summary>
			/// Subscription Name
			/// </summary>
			public string SubscriptionName { get; set; }

			/// <summary>
			/// Product Name
			/// </summary>
			public string ProductName { get; set; }

			/// <summary>
			/// Contains the URL of the ManagePermisions page  
			/// </summary>
			public string ManagePermissionsUrl { get; set; }
		}
	}

	/// <summary>
	/// Row in
	/// </summary>
	public class UserPermssionViewModel
	{
		/// <summary>
		/// UserId
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the email of the user.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Current product Role
		/// </summary>
		public int CurrentRole { get; set; }

		/// <summary>
		///
		/// </summary>
		public string CurrentRoleName { get; set; }

		/// <summary>
		/// User First Name
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// Is selected for the action
		/// </summary>
		public bool IsChecked { get; set; }
	}
}