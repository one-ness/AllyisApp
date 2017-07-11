
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
    {
		/// <summary>
		/// GET: /Account/EditSubscription.
		/// </summary>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public ActionResult EditSubscription(/*int id*/)
        {
			//int orgId = AppService.UserContext.UserSubscriptions[id].OrganizationId;
			//this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, orgId);
			return this.View(ViewConstants.EditSubscription);
		}
    }
}