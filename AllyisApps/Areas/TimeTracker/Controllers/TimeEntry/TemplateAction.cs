//------------------------------------------------------------------------------
// <copyright file="TemplateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Downloads the TimeEntryImportTemplate.csv file.
		/// </summary>
		/// <returns>A file object of TimeEntryImportTemplate.csv.</returns>
		public ActionResult Template()
		{
			byte[] fileBytes = (byte[])Resources.Files.Files.ResourceManager.GetObject("TimeEntryImportTemplate");
			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TimeEntryImportTemplate.xlsx");
		}
	}
}