//------------------------------------------------------------------------------
// <copyright file="UnsubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Subscription/Subscribe/ProductId=#.
		/// </summary>
		/// <param name="id">subcription id</param>
		/// <param name="skuId">The id of the SKU being subscribed to.</param>
		/// <returns>The result of this action.</returns>
		[HttpGet]
		public ActionResult Upgrade(int id, int skuId)
		{
			int orgId = AppService.UserContext.UserSubscriptions[id].OrganizationId;
			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, orgId);
			Task.Factory.StartNew(() => AppService.UpgradeSubscription(id, skuId));

			Notifications.Add(new BootstrapAlert(Resources.Strings.UpdateProfileSuccessMessage, Variety.Success));

			return this.RedirectToAction(ActionConstants.ManageOrg, ControllerConstants.Account, new { orgId });
		}
	}
}