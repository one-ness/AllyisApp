//------------------------------------------------------------------------------
// <copyright file="UserOrganization.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Auth
{
	/// <summary>
	/// an organization that a user belongs to
	/// </summary>
	public class UserOrganization : Organization
	{
		public int UserId { get; set; }
		public string EmployeeId { get; set; }
		public OrganizationRoleEnum OrganizationRole { get; set; }
		public decimal? MaxApprovalAmount { get; set; }
		public DateTime JoinedDateUtc { get; set; }
	}
}
