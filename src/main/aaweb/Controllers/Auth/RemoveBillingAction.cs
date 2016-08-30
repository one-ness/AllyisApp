//------------------------------------------------------------------------------
// <copyright file="RemoveBillingAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.ViewModels;

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
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization))
			{
				IEnumerable<int> subs = CrmService.GetSubscriptionPlanPrices();

				if (subs != null && subs.Count() > 0)
				{
					Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.CannotRemoveBilling, Variety.Warning));
					return this.Redirect(ActionConstants.Manage);
				}
				else
				{
					return this.View(ViewConstants.ConfirmRemoveBillingInformation);
				}
			}

			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction(ActionConstants.OrgIndex);
		}

		/// <summary>
		/// Removes Billing information.
		/// </summary>
		/// <param name="m">The model.</param>
		/// <returns>Action Result.</returns>
		[HttpPost]
		public ActionResult RemoveBilling(BaseViewModel m)
		{
			if (CrmService.RemoveBilling())
			{
				Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.BillingRemoved, Variety.Success));
				return this.Redirect(ActionConstants.Manage);
			}

			return this.RedirectToAction(ActionConstants.OrgIndex);
		}
	}
}