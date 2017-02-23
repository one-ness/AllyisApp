//------------------------------------------------------------------------------
// <copyright file="TemplateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Downloads the UserImportTemplate.csv file.
		/// </summary>
		/// <returns>A file object of UserImportTemplate.csv.</returns>
		public ActionResult Template()
		{
			byte[] fileBytes = (byte[])Resources.Files.Files.ResourceManager.GetObject("UserImportTemplate");
			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
		}
	}
}
