namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// Represents a pending invitation to an organization.
	/// </summary>
	public class InvitationViewModel
	{
		/// <summary>
		/// Gets or sets The Id for the invitation.
		/// </summary>
		public int InvitationId { get; set; }

		/// <summary>
		/// Gets or sets The Id of the organization associated with the invitation.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets The name of the organization associated with the invitation.
		/// </summary>
		public string OrganizationName { get; set; }
	}
}
