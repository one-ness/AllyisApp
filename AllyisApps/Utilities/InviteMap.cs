//------------------------------------------------------------------------------
// <copyright file="InviteMap.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using AllyisApps.ViewModels.Auth;
using CsvHelper.Configuration;

namespace AllyisApps.Utilities
{
	/// <summary>
	/// Map for CVS file upload of users.
	/// </summary>
	[CLSCompliant(false)]
	public sealed class InviteMap : CsvClassMap<OrganizationAddMembersViewModel>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InviteMap" /> class.
		/// </summary>
		public InviteMap()
		{
			Map(m => m.FirstName);
			Map(m => m.LastName);
			Map(m => m.Email);
		}
	}
}