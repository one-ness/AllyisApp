//------------------------------------------------------------------------------
// <copyright file="SkusAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.ViewModels.Billing;
using AllyisApps.Services.Billing;
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
		public async Task<ActionResult> Skus(int id)
		{
			var model = new SkusViewModel();
			model.CanSubscribe = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Create, AppService.AppEntity.Subscription, id);
			model.OrganizationId = id;
			var collection = AppService.GetAllActiveProductsAndSkus();
			foreach (var item in collection)
			{
				var pi = new SkusViewModel.ProductItemViewModel();
				pi.ProductName = item.ProductName;

				model.Products.Add(pi);
			}

			return View(model);
		}
	}
}
