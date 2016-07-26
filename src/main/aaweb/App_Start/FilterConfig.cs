//------------------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.Web.Mvc;
using AllyisApps.Utilities;

namespace AllyisApps
{
	/// <summary>
	/// Configures the application filters.
	/// </summary>
	public static class FilterConfig
	{
		/// <summary>
		/// Registers global attributes for all application classes and methods.
		/// </summary>
		/// <param name="filters">The existing filter collection in which to add.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "MVC framework impl.")]
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			// Default all pages and actions to authorization required.
			filters.Add(new AuthorizeAttribute());

			// Default all pages to handle errors.
			filters.Add(new ErrorHandler());
		}
	}
}
