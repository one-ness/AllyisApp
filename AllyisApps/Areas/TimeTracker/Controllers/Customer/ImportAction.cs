//------------------------------------------------------------------------------
// <copyright file="ImportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Expense;
using AllyisApps.Utilities;
using ExcelDataReader;
using System.Web;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// Directs the user to the customer import view.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <returns>the view to the customer import view.</returns>
		[HttpGet]
		public ActionResult CustomerImport(int subscriptionId)
		{
			ViewData["IsManager"] = AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			ViewData["SubscriptionId"] = subscriptionId;
			ViewData["SubscriptionName"] = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].SubscriptionName;
			ViewData["UserId"] = AppService.UserContext.UserId;
			return View();
		}

		/// <summary>
		/// POST: Customer/Import.
		/// Code adapted from http://techbrij.com/read-excel-xls-xlsx-asp-net-mvc-upload.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <param name="file">File to upload.</param>
		/// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CustomerImport(int subscriptionId, HttpPostedFileBase file)
		{
			// TODO: Replace ModelState errors with exception catches and notifications
			// TODO: Buff up the error handling (catch errors from import functions, etc.)
			if (!ModelState.IsValid)
			{
				return RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId });
			}

			if (AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId != ProductIdEnum.StaffingManager)
			{
				AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditCustomer, subscriptionId);
			}

			if (file != null && file.ContentLength > 0)
			{
				// ExcelDataReader works with the binary Excel file, so it needs a FileStream
				// to get started. This is how we avoid dependencies on ACE or Interop:
				Stream stream = file.InputStream;

				// We return the interface, so that
				IExcelDataReader reader;

				if (file.FileName.EndsWith(".xls"))
				{
					reader = ExcelReaderFactory.CreateBinaryReader(stream);
				}
				else if (file.FileName.EndsWith(".xlsx"))
				{
					reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Strings.FileFormatUnsupported, Variety.Danger));
					return RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId });
				}

				//reader.IsFirstRowAsColumnNames = true;
				//DataSet result = reader.AsDataSet();
				DataSet result = new DataSet();
				reader.Close();

				string[] formattedResult = ImportMessageFormatter.FormatImportResult(await AppService.Import(
					result,
					subscriptionId,
					inviteUrl: Url.Action(ActionConstants.Index, ControllerConstants.Account, null, Request.Url.Scheme)));
				if (!string.IsNullOrEmpty(formattedResult[0]))
				{
					Notifications.Add(new BootstrapAlert(formattedResult[0], Variety.Success) { IsHtmlString = true });
				}

				if (!string.IsNullOrEmpty(formattedResult[1]))
				{
					Notifications.Add(new BootstrapAlert(formattedResult[1], Variety.Warning) { IsHtmlString = true });
				}
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Strings.PleaseUploadFile, Variety.Danger));
			}

			return RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId });
		}
	}
}