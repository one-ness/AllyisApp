#pragma warning disable 1591

namespace AllyisApps.ViewModels.Auth
{
	/// <summary>
	/// view model for members and invitations tab
	/// </summary>
	public class MembersAndInvitationsTabViewModel
	{
		public int OrganizationId { get; set; }
		public string MembersTabActive { get; set; }
		public int MemberCount { get; set; }
		public string InvitationsTabActive { get; set; }
		public int PendingInvitationCount { get; set; }
		public string InvitationCountDisplay
		{
			get
			{
				var result = string.Empty;
				if (this.PendingInvitationCount > 0)
				{
					result = string.Format("({0})", this.PendingInvitationCount);
				}

				return result;
			}
		}
	}
}
