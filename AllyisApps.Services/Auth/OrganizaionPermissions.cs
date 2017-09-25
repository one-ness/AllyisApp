using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
