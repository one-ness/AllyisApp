using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// edit member view model
	/// </summary>
	public class EditMemberViewModel : BaseViewModel
	{
		/// <summary>
		/// user id
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// org id
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// organization name
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// Gets or sets the account e-mail.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the user's first name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the user's last name.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the user's phone number.
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the user's street address.
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the user's city.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the user's postal code.
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the state
		/// </summary>
		public string StateName { get; set; }

		/// <summary>
		/// Gets or sets the country
		/// </summary>
		public string CountryName { get; set; }

		/// <summary>
		/// Gets or sets Date of birth.
		/// </summary>
		public string DateOfBirth { get; set; } // has to be int for localization to work correctly. Gets changed to DateTime? when saving data from view.

		/// <summary>
		/// employee id
		/// </summary>
		[Display(Name = "Employee ID")]
		[Required]
		public string EmployeeId { get; set; }

		/// <summary>
		/// can edit member?
		/// </summary>
		public bool CanEditMember { get; set; }

		/// <summary>
		/// organization role id
		/// </summary>
		public int SelectedOrganizationRoleId { get; set; }

		/// <summary>
		/// list of org roles
		/// </summary>
		[Display(Name ="Organization Role")]
		
		public Dictionary<int, string> OrgRolesList { get; set; }

		/// <summary>
		/// subscription roles of this user
		/// </summary>
		public List<RoleItem> SubscriptionRoles { get; set; }

		/// <summary>
		/// role of a user
		/// </summary>
		public class RoleItem
		{
			/// <summary>
			/// subscription id
			/// </summary>
			public int SubscriptionId { get; set; }

			/// <summary>
			/// subscription name
			/// </summary>
			public string SubscriptionName { get; set; }

			/// <summary>
			/// role id
			/// </summary>
			public int SelectedRoleId { get; set; }

			/// <summary>
			/// list of role ids and names for the dropdown
			/// </summary>
			public Dictionary<int, string> RoleList { get; set; }

			/// <summary>
			/// constructor
			/// </summary>
			public RoleItem()
			{
				RoleList = new Dictionary<int, string>();
			}
		}

		/// <summary>
		/// constructor
		/// </summary>
		public EditMemberViewModel()
		{
			SubscriptionRoles = new List<RoleItem>();
		}
	}
}
