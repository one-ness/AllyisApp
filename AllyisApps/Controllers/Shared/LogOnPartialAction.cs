//------------------------------------------------------------------------------
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
		/// <param name="showOrganizationPartial">Whether to show the organization.</param>
		/// <returns>The ActionResult.</returns>
		[ChildActionOnly]
		public ActionResult LogOnPartial(string returnUrl, bool showOrganizationPartial = false)
		{
			LogOnPartialViewModel model = null;
			if (this.UserContext != null)
			{
				model = new LogOnPartialViewModel
				{
					UserName = UserContext.UserName,
					ChosenOrganizationId = UserContext.ChosenOrganizationId,
					ChosenOrganizationName = UserContext.ChosenOrganization.OrganizationName,
					CanEditOrganization = AppService.Can(Actions.CoreAction.EditOrganization, false),
					UserOrganizationBriefInfoList = new List<OrganizationBriefInfo>(),
					ShowOrganizationPartial = showOrganizationPartial
				};

				foreach (var orgInfo in UserContext.UserOrganizations.Values)
				{
					model.UserOrganizationBriefInfoList.Add(new OrganizationBriefInfo
					{
						OrganizationID = orgInfo.OrganizationId,
						OrganizationName = orgInfo.OrganizationName,
					});
				}
			}
			else
			{
				model = new LogOnPartialViewModel();
			}

			ViewBag.ReturnUrl = returnUrl;
			return this.View(ViewConstants.LogOnPartial, model);
		}
	}
}
