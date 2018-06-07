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
		/// Gets or sets the payclass id.
		/// </summary>
		public int PayClassId { get; set; }
		/// <summary>
		/// Gets or sets the built-in payclass this payclass corresponds to.
		/// </summary>
		public int BuiltinPayClassId { get; set; }

		/// <summary>
		/// Gets or sets payclass name.
		/// </summary>
		public string PayClassName { get; set; }

		/// <summary>
		/// Gets or sets organization this payclass belongs to.
		/// </summary>
		public int OrganizationId { get; set; }
	}
}
