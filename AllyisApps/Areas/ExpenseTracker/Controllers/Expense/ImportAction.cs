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
using AllyisApps.Services.Expense;

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
		/// <param name="reportId"></param>
		/// <param name="files">File to upload.</param>
		/// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Import(int subscriptionId, int reportId, List<ExpenseFile> files)
		{
			int organizationId = AppService.UserContext.OrganizationSubscriptions[subscriptionId].OrganizationId;

			// TODO: Replace ModelState errors with exception catches and notifications
			// TODO: Buff up the error handling (catch errors from import functions, etc.)
			if (ModelState.IsValid && files != null)
			{
				foreach (var upload in files)
				{
					AppService.CreateExpenseFile(upload, reportId);
				}
			}

			return RedirectToAction(ActionConstants.Create, ControllerConstants.Expense, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}
	}
}
