//------------------------------------------------------------------------------
// <copyright file="PermissionsActionsResults.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace AllyisApps.BusinessObjects
{
	/// <summary>
	/// Object for all results related to the actions that were requested.
	/// </summary>
	public class PermissionsActionsResults
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PermissionsActionsResults" /> class.
		/// </summary>
		public PermissionsActionsResults()
		{
			this.Results = new List<PermissionsActionResult>();
		}

		/// <summary>
		/// Gets or sets the overall status of the requested actions.
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the message to respond with.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the list of Result objects that were created when executing the actions.
		/// </summary>
		public List<PermissionsActionResult> Results { get; set; }

		/// <summary>
		/// Gets or sets the result string to respond with.
		/// </summary>
		public string Result { get; set; }
	}
}