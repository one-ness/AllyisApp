//------------------------------------------------------------------------------
// <copyright file="PayClass.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.Hrm
{
	/// <summary>
	/// An object for keeping track of all the info related to a given pay class.
	/// </summary>
	public class PayClass
	{
		/// <summary>
		/// Gets or sets the pay class id.
		/// </summary>
		public int PayClassId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string PayClassName { get; set; }

		/// <summary>
		/// Gets or sets the organization Id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the built-in payclass id
		/// </summary>
		[CLSCompliant(false)]
		public BuiltinPayClassEnum BuiltInPayClassId { get; set; }
	}
}