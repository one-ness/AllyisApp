//------------------------------------------------------------------------------
// <copyright file="LogOnPartialAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.ViewModels;

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
					ChosenOrganizationId = UserContext.ChosenOrganizationId,
					ChosenOrganizationName = UserContext.UserOrganizationInfoList.Where(o => o.OrganizationId == UserContext.ChosenOrganizationId).Select(o => o.OrganizationName).FirstOrDefault(),
					CanEditOrganization = AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization, false),
					UserOrganizationBriefInfoList = new List<OrganizationBriefInfo>()
				};

				foreach (var orgInfo in UserContext.UserOrganizationInfoList)
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