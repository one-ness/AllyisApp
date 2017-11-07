//------------------------------------------------------------------------------
// <copyright file="UpdateLanguageAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

namespace AllyisApps.Controllers.Shared
{
	/// <summary>
	/// Controller for actions relating to shared components of the page.
	/// </summary>
	public partial class SharedController : BaseController
	{
		/// <summary>
		/// Sets the language preference.
		/// </summary>
		/// <param name="cultureName">The language selection.</param>
		/// <returns>A redirection to the same page again.</returns>
		public ActionResult UpdateLanguage(string cultureName)
		{
			if (Request.IsAuthenticated)
			{
				AppService.SetLanguage(cultureName);
			}
			else
			{
				TempData[LanguageKey] = cultureName;
			}

			return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri); // Reloads page request came from
		}
	}
}