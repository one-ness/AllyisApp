using AllyisApps.Services;
using AllyisApps.Services.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// The view model for the Add Member page.
	/// </summary>
	public class AddMemberViewModel
	{
		/// <summary>
		/// Gets or sets the next recommended employee id.
		/// </summary>
		public string RecommendedEmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the list of subscriptions for the organization.
		/// </summary>
		public List<AddMemberSubscriptionInfo> Subscriptions { get; set; }

		/// <summary>
		/// Gets or sets the list of projects in the organization.
		/// </summary>
		public List<CompleteProjectInfo> Projects { get; set; }

		// ---------- FORM FIELDS -------------
		/// <summary>
		/// Gets or sets the organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the employee id.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to add as an owner.
		/// </summary>
		public bool AddAsOwner { get; set; }

	}

	/// <summary>
	/// A sub-view model for relevant subscription information on the Add Member page
	/// </summary>
	public class AddMemberSubscriptionInfo
	{
		/// <summary>
		/// Gets or sets the subscription id.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the product name.
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Gets or sets the list of product roles.
		/// </summary>
		public List<SubscriptionRoleInfo> ProductRoles { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this subscription is full.
		/// </summary>
		public bool hasTooManySubscribers { get; set; }
	}
}