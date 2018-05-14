//------------------------------------------------------------------------------
// <copyright file="PayClassDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Hrm
{
	/// <summary>
	/// Pay class.
	/// </summary>
	public class PayClassDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets.
		/// </summary>
		public int PayClassId { get; set; }
		/// <summary>
		/// Gets or sets int to identify the payclass if it is among the main entries.
		/// </summary>
		public int BuiltInPayClassId { get; set; }

		/// <summary>
		/// Gets or sets.
		/// </summary>
		public string PayClassName { get; set; }

		/// <summary>
		/// Gets or sets.
		/// </summary>
		public int OrganizationId { get; set; }
	}
}