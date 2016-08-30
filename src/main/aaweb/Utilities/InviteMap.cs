//------------------------------------------------------------------------------
// <copyright file="InviteMap.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.ViewModels;
using CsvHelper.Configuration;
using System;

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
			this.Map(m => m.FirstName);
			this.Map(m => m.LastName);
			this.Map(m => m.Email);
		}
	}
}