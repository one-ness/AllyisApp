//------------------------------------------------------------------------------
// <copyright file="DownloadImportUsersTemplateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Core;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Downloads the ImportUsersTemplate.csv file.
		/// </summary>
		/// <returns>A file object of ImportUsersTemplate.csv.</returns>
		public ActionResult DownloadImportUsersTemplate()
        {
            string dir = (string)Resources.Files.Files.ResourceManager.GetObject("ImportUserTemplate");
            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = "ImportUsersTemplate.csv",
                Inline = false
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return this.File(new System.Text.UTF8Encoding().GetBytes(dir), "text/csv");
		}
	}
}
