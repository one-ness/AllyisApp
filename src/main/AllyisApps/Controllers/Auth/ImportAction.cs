//------------------------------------------------------------------------------
// <copyright file="ImportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Utilities;

using Excel;

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
        /// <param name="upload">File to upload.</param>
        /// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(HttpPostedFileBase upload)
        {
            // TODO: Replace ModelState errors with exception catches and notifications
            // TODO: Buff up the error handling (catch errors from import functions, etc.)
            if (ModelState.IsValid)
            {
                if (Service.Can(Actions.CoreAction.EditOrganization))
                {
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
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View();
                        }

                        reader.IsFirstRowAsColumnNames = true;

                        DataSet result = reader.AsDataSet();
                        reader.Close();
                        
                        string[] formattedResult = ImportMessageFormatter.FormatImportResult(Service.Import(result));
                        if (!string.IsNullOrEmpty(formattedResult[0]))
                        {
                            Notifications.Add(new Core.Alert.BootstrapAlert(formattedResult[0], Core.Alert.Variety.Success));
                        }

                        if (!string.IsNullOrEmpty(formattedResult[1]))
                        {
                            Core.Alert.BootstrapAlert alert = new Core.Alert.BootstrapAlert(formattedResult[1], Core.Alert.Variety.Warning);
                            alert.IsHtmlString = true;
                            Notifications.Add(alert);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }
            }

            return RedirectToAction(ActionConstants.Add, ControllerConstants.Account);
        }
    }
}