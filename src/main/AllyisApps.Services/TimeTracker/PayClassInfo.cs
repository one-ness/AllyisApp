//------------------------------------------------------------------------------
// <copyright file="PayClassInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.TimeTracker
{
	/// <summary>
	/// An object for keeping track of all the info related to a given pay class.
	/// </summary>
	public class PayClassInfo
	{
		/// <summary>
		/// Gets or sets the pay class id.
		/// </summary>
		public int PayClassID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the organization ID.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Created time.
		/// </summary>
		public DateTime CreatedUTC { get; set; }

		/// <summary>
		/// Gets or sets the modified time.
		/// </summary>
		public DateTime ModifiedUTC { get; set; }
	}
}