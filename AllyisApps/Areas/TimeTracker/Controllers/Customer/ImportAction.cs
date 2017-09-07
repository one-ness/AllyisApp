//------------------------------------------------------------------------------
// <copyright file="ImportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Data;
using System.IO;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Expense;
using AllyisApps.Utilities;
using Excel;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// POST: Customer/Import.
		/// Code adapted from http://techbrij.com/read-excel-xls-xlsx-asp-net-mvc-upload.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <param name="file">File to upload.</param>
		/// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Import(int subscriptionId, ExpenseFile file)
		{
			// TODO: Replace ModelState errors with exception catches and notifications
			// TODO: Buff up the error handling (catch errors from import functions, etc.)
			if (ModelState.IsValid)
			{
				this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditCustomer, subscriptionId);
				if (file != null && file.ContentLength > 0)
				{
					// ExcelDataReader works with the binary Excel file, so it needs a FileStream
					// to get started. This is how we avoid dependencies on ACE or Interop:
					Stream stream = file.InputStream;

					// We return the interface, so that
					IExcelDataReader reader = null;

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
						Notifications.Add(new BootstrapAlert(Resources.Strings.FileFormatUnsupported, Variety.Danger));
						return RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = subscriptionId });
					}

					reader.IsFirstRowAsColumnNames = true;

					DataSet result = reader.AsDataSet();
					reader.Close();

					string[] formattedResult = ImportMessageFormatter.FormatImportResult(AppService.Import(result, subscriptionId: subscriptionId));
					if (!string.IsNullOrEmpty(formattedResult[0]))
					{
						Notifications.Add(new BootstrapAlert(formattedResult[0], Variety.Success));
					}

					if (!string.IsNullOrEmpty(formattedResult[1]))
					{
						BootstrapAlert alert = new BootstrapAlert(formattedResult[1], Variety.Warning);
						alert.IsHtmlString = true;
						Notifications.Add(alert);
					}
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.PleaseUploadFile, Variety.Danger));
				}
			}

			return RedirectToAction(ActionConstants.Index, ControllerConstants.Customer, new { subscriptionId = subscriptionId });
		}
	}
}
