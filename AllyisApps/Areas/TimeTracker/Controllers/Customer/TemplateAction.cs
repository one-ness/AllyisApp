//------------------------------------------------------------------------------
// <copyright file="TemplateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// Downloads the CustomersProjectsImportTemplate.csv file.
		/// </summary>
		/// <returns>A file object of CustomersProjectsImportTemplate.csv.</returns>
		public ActionResult Template()
		{
			byte[] fileBytes = (byte[])Resources.Files.Files.ResourceManager.GetObject("CustomerProjectImportTemplate");
			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerProjectImportTemplate.xlsx");
		}
	}
}
