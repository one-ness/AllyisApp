using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// organization members page
	/// </summary>
	public class OrganizationMembersViewModel2 : BaseViewModel
	{
		/// <summary>
		/// organization id
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// organization name
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// list of users in the organization
		/// </summary>
		public List<ViewModelItem> Users { get; set; }

		/// <summary>
		/// is delete allowed?
		/// </summary>
		public bool CanDeleteUser { get; set; }

		/// <summary>
		/// is manage permissions allowed?
		/// </summary>
		public bool CanManagePermissions { get; set; }

		/// <summary>
		/// is add user allowed?
		/// </summary>
		public bool CanAddUser { get; set; }

		/// <summary>
		/// is edit user allowed?
		/// </summary>
		public bool CanEditUser { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public OrganizationMembersViewModel2()
		{
			this.Users = new List<ViewModelItem>();
		}

		/// <summary>
		/// view model for list of users
		/// </summary>
		public class ViewModelItem
		{
			/// <summary>
			/// user id
			/// </summary>
			public int UserId { get; set; }
			/// <summary>
			/// full name
			/// </summary>
			public string Username { get; set; }

			/// <summary>
			/// email
			/// </summary>
			public string Email { get; set; }

			/// <summary>
			/// employee id
			/// </summary>
			public string EmployeeId { get; set; }
			
			/// <summary>
			/// joined date
			/// </summary>
			public DateTime JoinedDate { get; set; }

			/// <summary>
			/// formatted joined date
			/// </summary>
			public string FormattedJoinedDate
			{
				get
				{
					return this.JoinedDate.ToString("d");
				}
			}

			/// <summary>
			/// user role in the organization
			/// </summary>
			public string RoleName { get; set; }
		}
	}
}
