//------------------------------------------------------------------------------
// <copyright file="TemplateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
    /// <summary>
    /// Represents pages for the management of a Customer.
    /// </summary>
    public partial class CustomerController : BaseProductController
    {
        /// <summary>
		/// Downloads the CustomersProjectsImportTemplate.csv file.
		/// </summary>
		/// <returns>A file object of CustomersProjectsImportTemplate.csv.</returns>
		public ActionResult Template()
        {
            string dir = (string)Resources.Files.Files.ResourceManager.GetObject("CustomerProjectImportTemplate");
            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = "CustomerProjectImportTemplateTemplate.csv",
                Inline = false
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return this.File(new System.Text.UTF8Encoding().GetBytes(dir), "text/csv");
        }
    }
}