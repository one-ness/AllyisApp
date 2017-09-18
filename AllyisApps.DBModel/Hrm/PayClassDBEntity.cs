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
		/// Gets or sets.
		/// </summary>
		public string PayClassName { get; set; }

		/// <summary>
		/// Gets or sets.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets.
		/// </summary>
		public DateTime CreatedUtc { get; set; }
	}
}