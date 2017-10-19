namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// Invitation status used to
	/// </summary>
	public enum InvitationStatusEnum : int
	{
		/// <summary>
		/// any
		/// </summary>
		Any = 65535,

		/// <summary>
		/// Pending
		/// </summary>
		Pending = 1,

		/// <summary>
		/// Accepted
		/// </summary>
		Accepted = 2,

		/// <summary>
		/// Rejected
		/// </summary>
		Rejected = 4,
	}
}
