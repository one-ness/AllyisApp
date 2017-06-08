//------------------------------------------------------------------------------
// <copyright file="RemoveBillingAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Removes Billing information.
		/// </summary>
		/// <returns>Action result.</returns>
		[HttpGet]
		public ActionResult RemoveBilling()
		{
			if (AppService.Can(Actions.CoreAction.EditOrganization))
			{
				IEnumerable<int> subs = AppService.GetSubscriptionPlanPrices();

				if (subs != null && subs.Count() > 0)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.CannotRemoveBilling, Variety.Warning));
					return this.Redirect(ActionConstants.Manage);
				}
				else
				{
					return this.View(ViewConstants.ConfirmRemoveBillingInformation);
				}
			}

			Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction(ActionConstants.Index);
		}

		/// <summary>
		/// Removes Billing information.
		/// </summary>
		/// <param name="m">The model.</param>
		/// <returns>Action Result.</returns>
		[HttpPost]
		public ActionResult RemoveBilling(BaseViewModel m)
		{
			if (AppService.RemoveBilling())
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.BillingRemoved, Variety.Success));
				return this.Redirect(ActionConstants.Manage);
			}

			return this.RedirectToAction(ActionConstants.Index);
		}
	}
}
