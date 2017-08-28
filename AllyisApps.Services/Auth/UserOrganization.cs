//------------------------------------------------------------------------------
// <copyright file="UserOrganization.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AllyisApps.Services
{
	/// <summary>
	/// an organization that a user belongs to
	/// </summary>
	public class UserOrganization : Organization
	{
		public int UserId { get; set; }
		public string EmployeeId { get; set; }
		public OrganizationRole OrganizationRole { get; set; }
		public DateTime JoinedDateUtc { get; set; }
		public UserOrganization()
		{
			this.OrganizationRole = OrganizationRole.Member;
		}
	}
}
