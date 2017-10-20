using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// edit member view model
	/// </summary>
	public class EditMemberViewModel2 : BaseViewModel
	{
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
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the country
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets Date of birth.
		/// </summary>
		public string DateOfBirth { get; set; } // has to be int for localization to work correctly. Gets changed to DateTime? when saving data from view.

		/// <summary>
		/// employee id
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// roles of this user
		/// </summary>
		public List<RoleItem> Roles { get; set; }

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
			public int RoleId { get; set; }

			/// <summary>
			/// role name
			/// </summary>
			public string RoleName { get; set; }
		}

		/// <summary>
		/// constructor
		/// </summary>
		public EditMemberViewModel2()
		{
			this.Roles = new List<RoleItem>():
		}
	}
}
