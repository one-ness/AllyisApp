using System.Collections.Generic;
using AllyisApps.Services.Billing;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// Organizaion
	///
	/// </summary>
	public class OrganizaionPermissions
	{
		/// <summary>
		/// Contians the userRole of the members in the organizaion and thier roles for
		/// </summary>
		public List<UserRole> UserRoles { get; set; }

		/// <summary>
		/// Contains the subscriptions that
		/// </summary>
		public List<Subscription> Subscriptions { get; set; }
	}
}