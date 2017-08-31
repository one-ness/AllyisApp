//------------------------------------------------------------------------------
// <copyright file="HolidayDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.DBModel.Hrm
{
	/// <summary>
	/// Represents a holiday for an organization.
	/// </summary>
	public class HolidayDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the HolidayId.
		/// </summary>
		public int HolidayId { get; set; }

		/// <summary>
		/// Gets or sets the HolidayName.
		/// </summary>
		public string HolidayName { get; set; }

		/// <summary>
		/// Gets or sets the OrganizationId.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Date.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the CreatedUtc.
		/// </summary>
		public DateTime CreatedUtc { get; set; }
	}
}
