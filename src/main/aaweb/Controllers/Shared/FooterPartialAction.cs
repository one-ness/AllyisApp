//------------------------------------------------------------------------------
// <copyright file="FooterPartialAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.ViewModels;
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
			List<LanguageViewModel> model = AccountService.ValidLanguages().Select(l => new LanguageViewModel
			{
				LanguageID = l.LanguageID,
				LanguageName = l.LanguageName,
				CultureName = l.CultureName
			}).ToList();

			return this.View(ViewConstants.Footer, model);
		}
	}
}