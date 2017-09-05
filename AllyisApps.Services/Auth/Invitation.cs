using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
	/// <summary>
	/// invitations for a particular user
	/// </summary>
	public class Invitation
	{
		public string invitingOrgName { get; set; }
		public InvitationInfo invite;
	}
}
