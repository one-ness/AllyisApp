using AllyisApps.Services;
using AllyisApps.Services.Billing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// The view model for the Add Member page.
	/// </summary>
	public class AddMemberViewModel
	{
		/// <summary>
		/// Gets or sets the list of subscriptions for the organization.
		/// </summary>
		public List<AddMemberSubscriptionInfo> Subscriptions { get; set; }

		/// <summary>
		/// Gets or sets the list of projects in the organization.
		/// </summary>
		public List<CompleteProjectInfo> Projects { get; set; }

		/// <summary>
		/// Gets or sets the organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets user first name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Org.Strings)), ErrorMessageResourceName = "FirstNameValidation")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets user last name.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Org.Strings)), ErrorMessageResourceName = "LastNameValidation")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets UserInput.
		/// </summary>
		[Required(ErrorMessageResourceType = (typeof(AllyisApps.Resources.ViewModels.Org.Strings)), ErrorMessageResourceName = "EmailValidation")]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		[Required]
		[Display(Name = "Employee ID")]
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to add as an owner.
		/// </summary>
		public bool AddAsOwner { get; set; }

		/// <summary>
		/// Gets or sets the project id selected by the drop down menu.
		/// </summary>
		public int? SubscriptionProjectId { get; set; }
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
		public List<ProductRole> ProductRoles { get; set; }

		/// <summary>
		/// Gets or sets the role selected in the drop down menu.
		/// </summary>
		public int SelectedRole { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this subscription is full.
		/// </summary>
		public bool hasTooManySubscribers { get; set; }
	}
}
