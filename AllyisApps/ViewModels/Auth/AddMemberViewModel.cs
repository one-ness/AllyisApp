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
		/// Flags if the organization has a subscription to expense tracker.
		/// </summary>
		public bool hasET { get; set; }

		/// <summary>
		/// Gets or sets the Expense tracker role.
		/// </summary>
		public SelectList ETRoles { get; set; }

		/// <summary>
		/// Gets or sets the expense tracker selection value.
		/// </summary>
		public int etSelection { get; set; }

		/// <summary>
		/// Flags if the organization has a subscription to time tracker.
		/// </summary>
		public bool hasTT { get; set; }

		/// <summary>
		/// Gets or sets the Time tracker roles
		/// </summary>
		public SelectList TTRoles { get; set; }

		/// <summary>
		/// Gets or sets the time tracker selection value.
		/// </summary>
		public int ttSelection { get; set; }

		/// <summary>
		/// Gets or sets the Staffing manager roles.
		/// </summary>
		public SelectList SMRoles { get; set; }

		/// <summary>
		/// Gets or sets the staffing manager selection value.
		/// </summary>
		public int smSelection { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to add as an owner.
		/// </summary>
		public bool AddAsOwner { get; set; }
	}
}