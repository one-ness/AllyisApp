using System;
using System.Collections.Generic;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// organization members page
	/// </summary>
	public class OrganizationMembersViewModel : BaseViewModel
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
		/// paging: number of entries to show in one page
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// paging: current page number
		/// </summary>
		public int CurrentPageNumber { get; set; }

		/// <summary>
		/// paging: total pages
		/// </summary>
		public int TotalPages
		{
			get
			{
				return Users.Count / PageSize + 1;
			}
		}

		/// <summary>
		/// information for members and invitations tab
		/// </summary>
		public MembersAndInvitationsTabViewModel TabInfo { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public OrganizationMembersViewModel()
		{
			PageSize = 25; // default page number
			CurrentPageNumber = 1; // default page
			TabInfo = new MembersAndInvitationsTabViewModel();
			TabInfo.MembersTabActive = "active";
			Users = new List<ViewModelItem>();
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
					return JoinedDate.ToString("d");
				}
			}

			/// <summary>
			/// user role in the organization
			/// </summary>
			public string RoleName { get; set; }
		}
	}
}
