//------------------------------------------------------------------------------
// <copyright file="UpdateLanguageAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for actions relating to shared components of the page.
	/// </summary>
	public partial class SharedController : BaseController
	{
		/// <summary>
		/// Sets the language preference.
		/// </summary>
		/// <param name="languageID">The language selection.</param>
		/// <returns>A redirection to the same page again.</returns>
		public ActionResult UpdateLanguage(int languageID)
		{
			if (Request.IsAuthenticated)
			{
                AppService.SetLanguage(languageID);
			}
			else
			{
				TempData["language"] = languageID;
			}

			return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri); // Reloads page request came from
		}
	}
}
