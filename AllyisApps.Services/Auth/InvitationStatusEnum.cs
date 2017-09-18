namespace AllyisApps.Services
{
	/// <summary>
	/// Invitation status used to
	/// </summary>
	public enum InvitationStatusEnum : int
	{
		/// <summary>
		/// Pening invite accepted
		/// </summary>
		Pending = 0,

		/// <summary>
		/// Invite is accepted
		/// </summary>
		Accepted = 1,

		/// <summary>
		/// Invite is rejected
		/// </summary>
		Rejected = -1,
	}
}