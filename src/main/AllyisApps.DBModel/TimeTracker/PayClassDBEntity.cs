//------------------------------------------------------------------------------
// <copyright file="PayClassDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.TimeTracker
{
	/// <summary>
	/// Pay class.
	/// </summary>
	public class PayClassDBEntity
	{
		/// <summary>
		/// Gets or sets.
		/// </summary>
		public int PayClassID { get; set; }

		/// <summary>
		/// Gets or sets.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets.
		/// </summary>
		public DateTime CreatedUTC { get; set; }

		/// <summary>
		/// Gets or sets.
		/// </summary>
		public DateTime ModifiedUTC { get; set; }
	}
}