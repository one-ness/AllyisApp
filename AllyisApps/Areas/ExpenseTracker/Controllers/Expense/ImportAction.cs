//------------------------------------------------------------------------------
// <copyright file="ImportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;

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
			
            // TODO: Replace ModelState errors with exception catches and notifications
			// TODO: Buff up the error handling (catch errors from import functions, etc.)
			if (ModelState.IsValid)
			{
			}

			return RedirectToAction(ActionConstants.Index, ControllerConstants.Expense, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}
	}
}
