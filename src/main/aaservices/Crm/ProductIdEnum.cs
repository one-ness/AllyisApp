//------------------------------------------------------------------------------
// <copyright file="ProductIdEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.Crm
{
	/// <summary>
	/// List of products offered by us. These should match the product ids in the database.
	/// </summary>
	public enum ProductIdEnum : int
	{
		/// <summary>
		/// ID for TimeTracker.
		/// </summary>
		TimeTracker = 1,

		/// <summary>
		/// ID for Consulting.
		/// </summary>
		Consulting,
	}
}
