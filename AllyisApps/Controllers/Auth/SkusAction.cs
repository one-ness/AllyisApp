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
using AllyisApps.ViewModels.Billing;
using System.Threading.Tasks;

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
		/// <param name="id">The organization id.</param>
		/// <returns>The skus view.</returns>
		async public Task<ActionResult> Skus(int id)
		{
			var model = new SkusViewModel();
			model.CanSubscribe = await this.AppService.CheckOrgAction(AppService.OrgAction.CreateSubscription, id);
			model.OrganizationId = id;
			var collection = await this.AppService.GetAllActiveProductsAndSkus();
			foreach (var item in collection)
			{
				var pi = new SkusViewModel.ProductItem();
				pi.ProductName = item.ProductName;
				foreach (var sku in item.Skus)
				{
					var si = new SkusViewModel.ProductItem.SkuItem();
					si.Price = sku.CostPerUnit;
					si.SkuDescription = sku.SkuDescription;
					si.SkuIconUrl = sku.IconUrl;
					si.SkuId = (int)sku.SkuId;
					si.SkuName = sku.SkuName;
					pi.Skus.Add(si);
				}

				model.Products.Add(pi);
			}

			return View(model);
		}
	}
}
