//------------------------------------------------------------------------------
// <copyright file="OrganizationsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using AllyisApps.ViewModels.Shared;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get /Account/Organizations
		/// View a list of organizations.
		/// </summary>
		/// <returns>What to do.</returns>
		public ActionResult Organizations()
		{
            List<SubscriptionsViewModel> modelList = new List<SubscriptionsViewModel>();

            IEnumerable<OrganizationInfo> orgs = Service.GetOrganizationsByUserId();
            List<ProductInfo> productList = Service.GetProductInfoList();
            foreach (OrganizationInfo org in orgs)
            {
                modelList.Add(new SubscriptionsViewModel
                {
                    Subscriptions = Service.GetSubscriptionsDisplay(org.OrganizationId),
                    ProductList = productList,
                    OrgInfo = org,
                    CanEditOrganization = Service.Can(Actions.CoreAction.EditOrganization, false, org.OrganizationId),
                    TimeTrackerViewSelf = Service.Can(Actions.CoreAction.TimeTrackerEditSelf, false, org.OrganizationId)
                });
            }

            AccountOrgsViewModel model = new AccountOrgsViewModel
            {
                Organizations = modelList
            };

            ViewBag.ShowOrganizationPartial = false;
            return this.View(model);
		}
	}
}