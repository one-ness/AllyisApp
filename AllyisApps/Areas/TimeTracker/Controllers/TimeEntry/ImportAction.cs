//------------------------------------------------------------------------------
// <copyright file="ImportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Utilities;
using Excel;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// POST: TimeEntry/Import
		/// Code adapted from http://techbrij.com/read-excel-xls-xlsx-asp-net-mvc-upload.
		/// </summary>
		/// <param name="upload">File to upload.</param>
		/// <param name="subscriptionId">The subscription Id.</param>
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
				this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);

				if (upload != null && upload.ContentLength > 0)
				{
					// ExcelDataReader works with the binary Excel file, so it needs a FileStream
					// to get started. This is how we avoid dependencies on ACE or Interop:
					Stream stream = upload.InputStream;

					// We return the interface, so that
					IExcelDataReader reader = null;

					if (upload.FileName.EndsWith(".xls"))
					{
						reader = ExcelReaderFactory.CreateBinaryReader(stream);
					}
					else if (upload.FileName.EndsWith(".xlsx"))
					{
						reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
					}
					else
					{
						Notifications.Add(new BootstrapAlert(Resources.Strings.FileFormatUnsupported, Variety.Danger));
						return RedirectToAction(ActionConstants.Index, ControllerConstants.TimeEntry, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
					}

					reader.IsFirstRowAsColumnNames = true;

					DataSet result = reader.AsDataSet();
					reader.Close();

					string[] formattedResult = ImportMessageFormatter.FormatImportResult(AppService.Import(result, subscriptionId: subscriptionId));
					if (!string.IsNullOrEmpty(formattedResult[0]))
					{
						Notifications.Add(new AllyisApps.Core.Alert.BootstrapAlert(formattedResult[0], AllyisApps.Core.Alert.Variety.Success));
					}

					if (!string.IsNullOrEmpty(formattedResult[1]))
					{
						AllyisApps.Core.Alert.BootstrapAlert alert = new AllyisApps.Core.Alert.BootstrapAlert(formattedResult[1], AllyisApps.Core.Alert.Variety.Warning);
						alert.IsHtmlString = true;
						Notifications.Add(alert);
					}
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.PleaseUploadFile, Variety.Danger));
				}
			}

			return RedirectToAction(ActionConstants.Index, ControllerConstants.TimeEntry, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}
	}
}
