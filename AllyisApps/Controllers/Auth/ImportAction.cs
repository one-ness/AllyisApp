//------------------------------------------------------------------------------
// <copyright file="ImportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Utilities;
using Excel;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// POST: /Import
		/// For importing users into an organization
		/// Code adapted from http://techbrij.com/read-excel-xls-xlsx-asp-net-mvc-upload.
		/// </summary>
		/// <param name="id">Organization id.</param>
		/// <param name="upload">File to upload.</param>
		/// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Import(int id, HttpPostedFileBase upload)
		{
			// TODO: Replace ModelState errors with exception catches and notifications
			// TODO: Buff up the error handling (catch errors from import functions, etc.)
			if (ModelState.IsValid)
			{
				this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
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
						return RedirectToAction(ActionConstants.AddMember, ControllerConstants.Account, new { id = id });
					}

					reader.IsFirstRowAsColumnNames = true;

					DataSet result = reader.AsDataSet();
					reader.Close();

					string[] formattedResult = ImportMessageFormatter.FormatImportResult(AppService.Import(result, organizationId: id));
					if (!string.IsNullOrEmpty(formattedResult[0]))
					{
						Notifications.Add(new BootstrapAlert(formattedResult[0], Variety.Success));
					}

					if (!string.IsNullOrEmpty(formattedResult[1]))
					{
						Core.Alert.BootstrapAlert alert = new BootstrapAlert(formattedResult[1], Variety.Warning);
						alert.IsHtmlString = true;
						Notifications.Add(alert);
					}
					return RedirectToAction(ActionConstants.ManageOrg, ControllerConstants.Account, new { id = id });
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.PleaseUploadFile, Variety.Danger));
				}
			}

			return RedirectToAction(ActionConstants.AddMember, ControllerConstants.Account, new { id = id });
		}
	}
}
