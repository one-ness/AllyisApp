//------------------------------------------------------------------------------
// <copyright file="HolidayInfo.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services.BusinessObjects
{
	/// <summary>
	/// An object for keeping track of all the info related to a given holiday.
	/// </summary>
	public class HolidayInfo
	{
		/// <summary>
		/// Gets or sets the Holiday id.
		/// </summary>
		public int HolidayId { get; set; }

		/// <summary>
		/// Gets or sets the Holiday name.
		/// </summary>
		public string HolidayName { get; set; }

		/// <summary>
		/// Gets or sets the Organizaiton that has reconized the id.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the Date of ID.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the Date that holliday was added. 
		/// </summary>
		public DateTime CreatedUTC { get; set; }

		/// <summary>
		/// Gets or sets the Date holdiay was last modified.
		/// </summary>
		public DateTime ModifiedUTC { get; set; }
	}
}
