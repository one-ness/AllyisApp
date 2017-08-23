using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using System.IO;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
    /// <summary>
    /// Expense controller.
    /// </summary>
    public partial class ExpenseController : BaseController
    {
        /// <summary>
        /// Downloads an attachment file.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns></returns>
        public ActionResult Download(int reportId, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            string contentType = AzureFiles.DownloadReportAttachment(reportId, fileName, stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, contentType, fileName);
        }
    }
}