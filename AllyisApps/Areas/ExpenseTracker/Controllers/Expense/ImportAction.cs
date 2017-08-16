//------------------------------------------------------------------------------
// <copyright file="ImportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using System.Collections.Generic;
using System.Linq;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
        /// <summary>
        /// POST: TimeEntry/Import
        /// Code adapted from http://techbrij.com/read-excel-xls-xlsx-asp-net-mvc-upload.
        /// </summary>
        /// <param name="subscriptionId">The subscription Id.</param>
        /// <param name="upload">File to upload.</param>
        /// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Import(int subscriptionId, HttpPostedFileBase upload)
		{
			int organizationId = AppService.UserContext.OrganizationSubscriptions[subscriptionId].OrganizationId;
			var report = AppService.GetExpenseReportByOrgId(organizationId).ToList();
			if (report != null)
			{
				var reportId = report.First().ExpenseReportId;

				// TODO: Replace ModelState errors with exception catches and notifications
				// TODO: Buff up the error handling (catch errors from import functions, etc.)
				if (ModelState.IsValid && upload != null)
				{
					AppService.CreateExpenseFile(upload, reportId);
				}
			}

			return RedirectToAction(ActionConstants.Create, ControllerConstants.Expense, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}
	}
}
