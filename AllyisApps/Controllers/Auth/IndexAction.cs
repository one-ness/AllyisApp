//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
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
		/// GET: /Account/Index.
		/// Displays the account index page.
		/// </summary>
		/// <returns>The async task responsible for this action.</returns>
		public ActionResult Index()
		{
			var infos = AppService.GetUserOrgsAndInvitationInfo();

			IndexAndOrgsViewModel model = new IndexAndOrgsViewModel
			{
				UserInfo = infos.Item1,
				InviteInfos = infos.Item3
			};

			model.OrgInfos = infos.Item2.Select(o =>
			{
				OrgWithSubscriptionsForUserViewModel orgVM = new OrgWithSubscriptionsForUserViewModel
				{
					OrgInfo = o,
					CanEditOrganization = this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, o.OrganizationId, false),
					Subscriptions = new List<SubscriptionDisplayViewModel>()
				};
				UserOrganization userOrgInfo = null;
				this.AppService.UserContext.UserOrganizations.TryGetValue(o.OrganizationId, out userOrgInfo);
				if (userOrgInfo != null)
				{
					foreach (UserSubscription userSubInfo in userOrgInfo.UserSubscriptions.Values)
					{
						orgVM.Subscriptions.Add(new SubscriptionDisplayViewModel
						{
							SubscriptionId = userSubInfo.SubscriptionId,
							ProductId = (int)userSubInfo.ProductId,
							ProductName = userSubInfo.ProductName,
							ProductDescription = userSubInfo.ProductId == ProductIdEnum.TimeTracker ? Resources.Strings.TimeTrackerDescription : "",
							AreaUrl = userSubInfo.AreaUrl
						});
					}
				}
				return orgVM;
			}).ToList();

			return this.View(model);
		}
	}
}
