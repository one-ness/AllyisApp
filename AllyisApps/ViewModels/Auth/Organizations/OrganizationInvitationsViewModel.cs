using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// invitations view model
	/// </summary>
	public class OrganizationInvitationsViewModel : BaseViewModel
	{
		/// <summary>
		/// organization id
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// name of the organization
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// list of invitations
		/// </summary>
		public List<ViewModelItem> Invitations { get; set; }

		/// <summary>
		/// list of invitations checked
		/// </summary>
		public string CheckedInvitations { get; set; }

		/// <summary>
		/// information for members and invitations tab
		/// </summary>
		public MembersAndInvitationsTabViewModel TabInfo { get; set; }

		/// <summary>
		/// is delete allowed?
		/// </summary>
		public bool CanDeleteInvitations { get; set; }

		/// <summary>
		/// is resend allowed?
		/// </summary>
		public bool CanResendInvitations { get; set; }

		/// <summary>
		/// constructor
		/// </summary>
		public OrganizationInvitationsViewModel()
		{
			this.Invitations = new List<ViewModelItem>();
			this.TabInfo = new MembersAndInvitationsTabViewModel();
			this.TabInfo.InvitationsTabActive = "active";
		}

		/// <summary>
		/// invitation
		/// </summary>
		public class ViewModelItem
		{
			/// <summary>
			/// invitation id
			/// </summary>
			public int InvitationId { get; set; }

			/// <summary>
			/// invited user name
			/// </summary>
			public string Username { get; set; }

			/// <summary>
			/// invited user email
			/// </summary>
			public string Email { get; set; }

			/// <summary>
			/// employee id to be assigned to user
			/// </summary>
			public string EmployeeId { get; set; }

			/// <summary>
			/// products and roles they were invited to
			/// </summary>
			public List<Tuple<string, string>> ProductAndRoleNames { get; set; }

			/// <summary>
			/// status of the invitation, accepted/rejected/pending
			/// </summary>
			public string Status { get; set; }
 
			/// <summary>
			/// invitation date
			/// </summary>
			public DateTime InvitedOn { get; set; }

			/// <summary>
			/// formatted invitation date
			/// </summary>
			public string FormattedInvitedOnDate
			{
				get
				{
					return this.InvitedOn.ToString("d");
				}
			}

			/// <summary>
			/// date on which the status 
			/// </summary>
			public DateTime? DecisionDate { get; set; }

			/// <summary>
			/// constructor;
			/// </summary>
			public ViewModelItem()
			{
				this.ProductAndRoleNames = new List<Tuple<string, string>>();
			}
		}
	}
}
