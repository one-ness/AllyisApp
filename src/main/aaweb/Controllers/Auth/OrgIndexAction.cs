//------------------------------------------------------------------------------
// <copyright file="OrgIndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.ViewModels.Shared;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get: Account/OrgIndex.
		/// </summary>
		/// <returns>The organization's homepage.</returns>
		[HttpGet]
		public ActionResult OrgIndex()
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.ViewOrganization))
			{
				SubscriptionsViewModel subscriptions = new SubscriptionsViewModel
				{
					Subscriptions = CrmService.GetSubscriptionsDisplayByOrg(UserContext.ChosenOrganizationId),
					ProductList = Services.Crm.CrmService.GetProductInfoList(),
					OrgInfo = OrgService.GetOrganization(UserContext.ChosenOrganizationId),
					CanEditOrganization = AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization),
					TimeTrackerViewSelf = AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf)
				};
				
				return this.View(subscriptions);
			}

			Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			return this.RedirectToAction("Index", "Home");
		}
	}
}
