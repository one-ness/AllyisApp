﻿//------------------------------------------------------------------------------
// <copyright file="DownloadImportUsersTemplateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Controllers.Auth
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

			return File(new System.Text.UTF8Encoding().GetBytes(dir), "text/csv");
		}
	}
}