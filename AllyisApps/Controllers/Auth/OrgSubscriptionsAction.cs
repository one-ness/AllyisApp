//------------------------------------------------------------------------------
// <copyright file="OrgSubscriptionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;
using System.Web.Mvc;

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
			model.CanEditSubscriptions = this.AppService.CheckOrgAction(AppService.OrgAction.EditSubscription, id, false);
			model.CanManagePermissions = this.AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id, false);
			model.OrganizationId = id;
			var collection = await this.AppService.GetSubscriptionsAsync(id);
			foreach (var item in collection)
			{
				var data = new OrganizationSubscriptionsViewModel.ViewModelItem();
				data.AreaUrl = item.ProductAreaUrl;
				data.NumberofUsers = item.NumberOfUsers;
				data.ProductDescription = item.ProductDescription;
				data.ProductName = item.ProductName;
				data.SubscriptionCreatedUtc = item.SubscriptionCreatedUtc;
				data.SubscriptionId = item.SubscriptionId;
				data.SubscriptionName = item.SubscriptionName;
				model.Subscriptions.Add(data);
			}

			var org = await this.AppService.GetOrganization(id);
			model.OrganizationName = org.OrganizationName;

			return View(model);
		}
	}
}
