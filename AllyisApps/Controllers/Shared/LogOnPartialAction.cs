//------------------------------------------------------------------------------
// <copyright file="LogOnPartialAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.ViewModels.Shared;

namespace AllyisApps.Controllers.Shared
{
	/// <summary>
	/// Controller for actions relating to shared components of the page.
	/// </summary>
	public partial class SharedController : BaseController
	{
		/// <summary>
		/// Gets the log on partial.
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns>The ActionResult.</returns>
		[ChildActionOnly]
		public ActionResult LogOnPartial(string returnUrl)
		{
			LogOnPartialViewModel model = null;
			if (AppService.UserContext != null)
			{
				model = new LogOnPartialViewModel
				{
					FirstName = AppService.UserContext.FirstName,
					LastName = AppService.UserContext.LastName
				};
			}
			else
			{
				model = new LogOnPartialViewModel();
			}

			ViewBag.ReturnUrl = returnUrl;
			return PartialView(ViewConstants.LogOnPartial, model);
		}
	}
}