using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
	/// <summary>
	/// user in an organization
	/// </summary>
	public class OrganizationUser : User
	{
		/// <summary>
		/// Gets or sets the Organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Organization role Id.
		/// </summary>
		public int OrganizationRoleId { get; set; }

		public string OrganizationRoleName { get; set; }

		/// <summary>
		/// Gets or sets the Date this user was added to the organization.
		/// </summary>
		public DateTime CreatedUtc { get; set; }

		/// <summary>
		/// Gets or sets the Employee Id.
		/// </summary>
		public string EmployeeId { get; set; }

	}
}
