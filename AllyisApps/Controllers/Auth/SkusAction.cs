//------------------------------------------------------------------------------
// <copyright file="SkusAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /account/skus/id.
		/// </summary>
		/// <param name="organizationId">The organization id.</param>
		/// <returns>The skus view.</returns>
		public ActionResult Skus(int organizationId)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, organizationId);    // only org owner has permission

			SkusListViewModel model = ConstructSkusListViewModel(organizationId);

			return this.View("Skus", model);
		}

		/// <summary>
		/// Uses services and utilities to initialize an <see cref="SkusListViewModel"/>.
		/// </summary>
		/// <param name="orgId">The current organization Id.</param>
		/// <returns>Populated SkusListViewModel.</returns>
		public SkusListViewModel ConstructSkusListViewModel(int orgId)
		{
			SkusListViewModel model = new SkusListViewModel { OrganizationId = orgId, ProductsList = new List<SkusListViewModel.ProductViewModel>() };

			var result = AppService.GetAllActiveProductsAndSkus();
			var activeSubscriptions = AppService.GetSubscriptionsDisplay(orgId);

			model.CurrentSubscriptions = activeSubscriptions.Select(sub => new SubscriptionDisplayViewModel(sub));

			model.ProductsList = result.Select(prod => new SkusListViewModel.ProductViewModel()
			{
				AreaUrl = prod.AreaUrl,
				ProductDescription = prod.ProductDescription,
				ProductId = prod.ProductId,
				ProductName = prod.ProductName,
				ProductSkus = prod.ProductSkus.Select(psku => new SkusListViewModel.ProductViewModel.SkuInfoViewModel()
				{
					Description = psku.Description,
					IconUrl = psku.IconUrl,
					Price = psku.Price,
					ProductId = psku.ProductId,
					SkuId = psku.SkuId,
					SkuIdNext = psku.SkuIdNext,
					SkuName = psku.SkuName
				}).ToList()
			});

			return model;
		}
	}
}