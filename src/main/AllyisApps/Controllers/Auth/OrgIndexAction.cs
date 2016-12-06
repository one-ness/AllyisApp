//------------------------------------------------------------------------------
// <copyright file="OrgIndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Core;
using AllyisApps.Services;
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
			if (Service.Can(Actions.CoreAction.ViewOrganization))
			{
				SubscriptionsViewModel subscriptions = new SubscriptionsViewModel
				{
					Subscriptions = Service.GetSubscriptionsDisplay(),
					ProductList = Service.GetProductInfoList(),
					OrgInfo = Service.GetOrganization(UserContext.ChosenOrganizationId),
					CanEditOrganization = Service.Can(Actions.CoreAction.EditOrganization),
					TimeTrackerViewSelf = Service.Can(Actions.CoreAction.TimeTrackerEditSelf)
				};

				return this.View(subscriptions);
			}

			Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
            return this.RouteHome();
		}
	}
}