

using AllyisApps.Services;
using System;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents a pending invitation to an organization.
	/// </summary>
	public class InvitationInfoViewModel
	{
		/// <summary>
		/// Gets or sets the Invitation Id.
		/// </summary>
		public int InvitationId { get; set; }

		/// <summary>
		/// Gets or sets the Email address of invitee.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Compressed version of email address, for display.
		/// </summary>
		public string CompressedEmail { get; set; }

		/// <summary>
		/// Gets or sets the First name.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the Last name.
		/// </summary>
		public string LastName { get; set; }



		/// <summary>
		/// Gets or sets the Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		public string EmployeeId { get; set; }

		/// <summary>
		/// Date that user had accepted or rejected the invitation.
		/// </summary>
		public DateTime DecisionDateUtc { get; set; }

		/// <summary>
		/// Invitation Info constuctor as it is likely that service object is reused in many places
		/// </summary>
		/// <param name="info"></param>
		public InvitationInfoViewModel(InvitationInfo info)
		{
			this.CompressedEmail = info.CompressedEmail;
			this.DecisionDateUtc = info.DecisionDateUtc;
			this.Email = info.Email;
			this.EmployeeId = info.EmployeeId;
			this.FirstName = info.FirstName;
			this.InvitationId = info.InvitationId;
			this.LastName = info.LastName;
			this.OrganizationId = info.OrganizationId;
		}
	}
}
