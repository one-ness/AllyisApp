//------------------------------------------------------------------------------
// <copyright file="OrgSubscriptionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get: Account/OrgSubscriptions
		/// </summary>
		public async Task<ActionResult> OrgSubscriptions(int id)
		{
			var model = new OrganizationSubscriptionsViewModel();
			model.CanEditSubscriptions = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Update, AppService.AppEntity.Subscription, id, false);
			model.CanManagePermissions = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Update, AppService.AppEntity.Permission, id, false);
			model.OrganizationId = id;
			var collection = await AppService.GetSubscriptionsAsync(id);
			foreach (Subscription item in collection)
			{
				var data = new OrganizationSubscriptionsViewModel.SubscriptionViewModel();
				data.AreaUrl = item.ProductAreaUrl;
				data.NumberofUsers = item.NumberOfUsers;
				data.ProductDescription = item.ProductDescription;
				data.ProductName = item.ProductName;
				data.SubscriptionCreatedUtc = item.CreatedUtc;
				data.SubscriptionId = item.SubscriptionId;
				data.SubscriptionName = item.SubscriptionName;
				data.PermissionsUrl = GetPermissionsUrl(item.ProductId, item.SubscriptionId);
				model.Subscriptions.Add(data);
			}

			var org = await AppService.GetOrganizationAsync(id);
			model.OrganizationName = org.OrganizationName;

			return View(model);
		}
	}
}