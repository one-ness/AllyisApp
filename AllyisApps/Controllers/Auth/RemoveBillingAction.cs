//------------------------------------------------------------------------------
// <copyright file="RemoveBillingAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels;
using System.Threading.Tasks;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Removes Billing information.
		/// </summary>
		/// <param name="id">Subscription plan id.</param>
		/// <returns>Action result.</returns>
		[HttpGet]
		async public Task<ActionResult> RemoveBilling(int id)
		{
			await this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
			IEnumerable<int> subs = await AppService.GetSubscriptionPlanPrices(id);

			if (subs != null && subs.Count() > 0)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotRemoveBilling, Variety.Warning));
				return this.Redirect(ActionConstants.ManageOrg);
			}
			else
			{
				return this.View(ViewConstants.ConfirmRemoveBillingInformation);
			}
		}

		/// <summary>
		/// Removes Billing information.
		/// </summary>
		/// <param name="m">The model.</param>
		/// <returns>Action Result.</returns>
		[HttpPost]
		async public Task<ActionResult> RemoveBilling(BaseViewModel m)
		{
			// TODO: org id is needed
			int orgId = 0;
			if (await AppService.RemoveBilling(orgId))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.BillingRemoved, Variety.Success));
				return this.Redirect(ActionConstants.ManageOrg);
			}

			return this.RedirectToAction(ActionConstants.Index);
		}
	}
}