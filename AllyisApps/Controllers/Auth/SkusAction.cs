//------------------------------------------------------------------------------
// <copyright file="SkusAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers
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
			this.AppService.CheckOrgAction(AppService.OrgAction.SubscribeToProduct, organizationId);    // only org owner has permission

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
			SkusListViewModel model = new SkusListViewModel { OrganizationId = orgId, ProductsList = new List<Product>() };

			var result = AppService.GetAllActiveProductsAndSkus();
			var activeSubscriptions = AppService.GetSubscriptionsDisplay(orgId);

			model.CurrentSubscriptions = activeSubscriptions;
			model.ProductsList = result.Item1;
			foreach (Product prod in model.ProductsList)
			{
				prod.ProductSkus = new List<SkuInfo>();
			}

			foreach (SkuInfo sku in result.Item2)
			{
				foreach (Product prod in model.ProductsList)
				{
					if (sku.ProductId == prod.ProductId)
					{
						prod.ProductSkus.Add(sku);
						break;
					}
				}
			}

			return model;
		}
	}
}
