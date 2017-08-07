﻿//------------------------------------------------------------------------------
// <copyright file="UpdateLanguageAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
		/// <param name="CultureName">The language selection.</param>
		/// <returns>A redirection to the same page again.</returns>
		public ActionResult UpdateLanguage(string CultureName)
		{
			if (Request.IsAuthenticated)
			{
				AppService.SetLanguage(CultureName);
			}
			else
			{
				TempData[languageKey] = CultureName;
			}

			return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri); // Reloads page request came from
		}
	}
}
