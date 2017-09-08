//------------------------------------------------------------------------------
// <copyright file="FooterPartialAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.ViewModels.Shared;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for actions relating to shared components of the page.
	/// </summary>
	public partial class SharedController : BaseController
	{
		/// <summary>
		/// Gets the footer partial.
		/// </summary>
		/// <returns>The ActionResult.</returns>
		[ChildActionOnly]
		public ActionResult FooterPartial()
		{
			List<LanguageViewModel> languages = AppService.ValidLanguages().Select(l => new LanguageViewModel
			{
				LanguageName = l.LanguageName,
				CultureName = l.CultureName
			}).ToList();
			var model = languages;
			ViewData["CultureName"] = AppService.UserContext != null ? AppService.UserContext.PreferedLanguageId : TempData["language"];
			return this.View(ViewConstants.Footer, model);
		}
	}
}
