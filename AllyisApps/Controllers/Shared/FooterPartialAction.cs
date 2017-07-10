//------------------------------------------------------------------------------
// <copyright file="FooterPartialAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
				LanguageID = l.LanguageId,
				LanguageName = l.LanguageName,
				CultureName = l.CultureName
			}).ToList();
			int orgID = this.AppService.UserContext == null ? 0 : this.AppService.UserContext.ChosenOrganizationId;
			var model = Tuple.Create(languages, orgID);
			ViewData["LanguageID"] = AppService.UserContext != null ? AppService.UserContext.ChosenLanguageId : TempData["language"];
			return this.View(ViewConstants.Footer, model);
		}
	}
}
