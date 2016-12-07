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

using Excel;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
    /// <summary>
    /// Represents pages for the management of a Customer.
    /// </summary>
    public partial class CustomerController : BaseProductController
    {
        /// <summary>
        /// POST: Customer/Import.
        /// Code adapted from http://techbrij.com/read-excel-xls-xlsx-asp-net-mvc-upload.
        /// </summary>
        /// <param name="FileType"></param>
        /// <param name="upload"></param>
        /// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(HttpPostedFileBase upload, string FileType)
        {
            // TODO: Replace ModelState errors with exception catches and notifications
            // TODO: Buff up the error handling (catch errors from import functions, etc.)
            if (ModelState.IsValid)
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

                    Service.Import(result);
                    // Import function depends on filetype selected by user
                    // Would love to have this be a Switch-Case, but the Resource strings aren't technically constants
                    /*
                    if (FileType == Resources.TimeTracker.Views.Customer.Strings.CustomersFile)
                        Service.ImportCustomers(result.Tables[0]);
                    else if (FileType == Resources.TimeTracker.Views.Customer.Strings.ProjectsFile)
                        Service.ImportProjects(result.Tables[0]);
                    else ModelState.AddModelError("File", "Please Select a File Type");
                    */
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return RedirectToAction(ActionConstants.Index, ControllerConstants.Customer);
        }
    }
}