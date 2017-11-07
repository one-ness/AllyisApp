using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// The view model for the Add Member page.
	/// </summary>
	public class AddMemberViewModel
	{
		/// <summary>
		/// Gets or sets the organization id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets user first name.
		/// </summary>
		[Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "FirstNameValidationAddMember")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets user last name.
		/// </summary>
		[Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "LastNameValidationAddMember")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets UserInput.
		/// </summary>
		[Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "EmailValidation")]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		[Required]
		[Display(Name = "Employee Id")]
		public string EmployeeId { get; set; }

		/// <summary>
		/// Gets or sets the organization role list.
		/// </summary>
		public SelectList OrgRole { get; set; }

		/// <summary>
		/// Gets or sets the organization role selection.
		/// </summary>
		public int OrgRoleSelection { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to add as an owner.
		/// </summary>
		public bool AddAsOwner { get; set; }

		/// <summary>
		/// Gets or sets the name of the organization.
		/// </summary>
		public string OrganizationName { get; internal set; }

		/// <summary>
		/// Gets or sets the subscription roles for the user.
		/// </summary>
		public List<RoleItem> SubscriptionRoles { get; set; }
	}

	/// <summary>
	/// role of a user
	/// </summary>
	public class RoleItem
	{
		/// <summary>
		/// Gets or sets the subscription name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets the selected role Id.
		/// </summary>
		public int SelectedRoleId { get; set; }

		/// <summary>
		/// Gets or sets the product Id.
		/// </summary>
		public int ProductId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets the select list for dropdown menus.
		/// </summary>
		public List<SelectListItem> SelectList { get; set; }
	}
}