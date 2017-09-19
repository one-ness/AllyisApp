//------------------------------------------------------------------------------
// <copyright file="UserOrganization.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// an organization that a user belongs to
	/// </summary>
	public class UserOrganization
	{
		public int UserId { get; set; }
		public string EmployeeId { get; set; }
		public OrganizationRole OrganizationRole { get; set; }
		public decimal MaxAmount { get; set; }
		public DateTime JoinedDateUtc { get; set; }
		public Organization Organization;

		public UserOrganization()
		{
			//this.OrganizationRole = OrganizationRole.Member; Not sure what this was used for should not be putting deflats into constuctor unless obvious.
		}
	}
}