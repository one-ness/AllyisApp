﻿//------------------------------------------------------------------------------
// <copyright file="LogOnPartialAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.ViewModels.Shared;
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
		/// Gets the log on partial.
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns>The ActionResult.</returns>
		[ChildActionOnly]
		public ActionResult LogOnPartial(string returnUrl)
		{
			LogOnPartialViewModel model = null;
			if (this.UserContext != null)
			{
				model = new LogOnPartialViewModel
				{
					UserName = UserContext.UserName,
                    FirstName = UserContext.FirstName,
                    LastName = UserContext.LastName,
					ChosenOrganizationName = UserContext.ChosenOrganization == null ? string.Empty : UserContext.ChosenOrganization.OrganizationName,
				};
			}
			else
			{
				model = new LogOnPartialViewModel();
			}

			ViewBag.ReturnUrl = returnUrl;
			return this.PartialView(ViewConstants.LogOnPartial, model);
		}
	}
}
