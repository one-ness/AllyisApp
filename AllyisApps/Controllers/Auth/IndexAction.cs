//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System;
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

            foreach(var invite in infos.Item3)
            {
                string orgName =  AppService.GetOrganization(invite.OrganizationId).OrganizationName;
                ViewData[invite.OrganizationId.ToString()] = orgName;
            }

			model.UserInfo.Address = infos.Item4;

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
					foreach (UserSubscription userSubInfo in userOrgInfo.OrganizationSubscriptions.Values)
					{
						//TODO move description info into a product description column in Billing.Product
						string description =
							userSubInfo.ProductId == ProductIdEnum.TimeTracker ? Resources.Strings.TimeTrackerDescription :
							userSubInfo.ProductId == ProductIdEnum.ExpenseTracker ? Resources.Strings.ExpenseTrackerDescription :
							"";

						if (userSubInfo.ProductId == ProductIdEnum.TimeTracker)
						{
							int startOfWeek = AppService.GetAllSettings(userSubInfo.SubscriptionId).Item1.StartOfWeek;
							ViewBag.StartDate = AppService.GetDayFromDateTime(SetStartingDate(startOfWeek));
							ViewBag.EndDate = AppService.GetDayFromDateTime(SetStartingDate(startOfWeek).AddDays(6));
						}

						orgVM.Subscriptions.Add(new SubscriptionDisplayViewModel
						{
							SubscriptionId = userSubInfo.SubscriptionId,
							SubscriptionName = userSubInfo.SubscriptionName,
							ProductId = (int)userSubInfo.ProductId,
							ProductName = userSubInfo.ProductName,
							ProductDescription = description,
							AreaUrl = userSubInfo.AreaUrl
						});
					}
				}
				return orgVM;
			}).ToList();

			return this.View(model);
		}

        private DateTime SetStartingDate(int startOfWeek)
        {
            DateTime today = DateTime.Now;
            int daysIntoTheWeek = (int)today.DayOfWeek < startOfWeek
                ? (int)today.DayOfWeek + (7 - startOfWeek)
                : (int)today.DayOfWeek - startOfWeek;




            return today.AddDays(-daysIntoTheWeek);
        }
    }
}
