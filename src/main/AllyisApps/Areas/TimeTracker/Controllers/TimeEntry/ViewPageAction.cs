//------------------------------------------------------------------------------
// <copyright file="ViewPageAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Submits page button press to view data.
		/// </summary>
		/// <param name="users">Array of selected user Ids.</param>
		/// <param name="startDate">The beginning of the date range(nullable).</param>
		/// <param name="endDate">The end of the date range (nullable).</param>
		/// <param name="showExport">Export button visibility.</param>
		/// <param name="customerSelect">The Customer's id (not required).</param>
		/// <param name="pageButton">The page of results to view.</param>
		/// <param name="projectSelect">The project's id (not required).</param>
		/// <returns>The selected page of preview data.</returns>
		[HttpPost]
		public ActionResult ViewPage(List<string> users, DateTime? startDate, DateTime? endDate, bool showExport, int customerSelect, int pageButton, int projectSelect = 0)
		{
			string viewDataButton = "Preview";
			int pageNum = pageButton;
			DateTime? dateRangeStart = startDate;
			DateTime? dateRangeEnd = endDate;
			List<string> userSelect = null;
			this.TempData["USelect"] = users;

			return this.RedirectToAction(ActionConstants.ViewReport, new { viewDataButton, userSelect, dateRangeStart, dateRangeEnd, showExport, customerSelect, pageNum, projectSelect });
		}
	}
}
