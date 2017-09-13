﻿//------------------------------------------------------------------------------
// <copyright file="Holiday.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// An object for keeping track of all the info related to a given holiday.
	/// </summary>
	public class Holiday
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
		/// Gets or sets the Date of Id.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the Date that holliday was added.
		/// </summary>
		public DateTime CreatedUtc { get; set; }
	}
}