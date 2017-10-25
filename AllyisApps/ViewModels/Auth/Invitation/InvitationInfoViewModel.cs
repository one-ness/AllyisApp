using System;
using AllyisApps.Services.Auth;

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents a pending invitation to an organization.
	/// </summary>
	public class InvitationInfoViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvitationInfoViewModel"/> class.
		/// Invitation Info constuctor as it is likely that service object is reused in many places.
		/// </summary>
		/// <param name="info">Invitation Infos.</param>
		public InvitationInfoViewModel(Invitation info)
		{
			CompressedEmail = info.CompressedEmail;
			DecisionDateUtc = info.DecisionDateUtc;
			Email = info.Email;
			EmployeeId = info.EmployeeId;
			FirstName = info.FirstName;
			InvitationId = info.InvitationId;
			LastName = info.LastName;
			OrganizationId = info.OrganizationId;
		}

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
		/// Gets or sets Date that user had accepted or rejected the invitation.
		/// </summary>
		public DateTime? DecisionDateUtc { get; set; }
	}
}