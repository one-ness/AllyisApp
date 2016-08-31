//------------------------------------------------------------------------------
// <copyright file="PermissionsActionResult.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.Services.Account
{
	/// <summary>
	/// An object representing the results of a PermissionsAction.
	/// </summary>
	public class PermissionsActionResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PermissionsActionResult" /> class.
		/// </summary>
		public PermissionsActionResult()
		{
			this.Users = new List<TargetUser>();
		}

		/// <summary>
		/// Gets or sets the count of users affected with this result.
		/// </summary>
		public int AffectedUserCount { get; set; }

		/// <summary>
		/// Gets or sets the total count of users who had the associated action performed.
		/// </summary>
		public int TotalUserCount { get; set; }

		/// <summary>
		/// Gets or sets the text response for this result.
		/// </summary>
		public string ActionText { get; set; }

		/// <summary>
		/// Gets or sets the status associated with this result.
		/// </summary>
		public string ActionStatus { get; set; }

		/// <summary>
		/// Gets or sets the list of users associated with this result.
		/// </summary>
		public List<TargetUser> Users { get; set; }
	}
}