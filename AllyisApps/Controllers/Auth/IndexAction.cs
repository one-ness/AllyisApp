//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.Services.Auth;
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
            AccountIndexViewModel viewModel = ConstuctIndexViewModel();
            //View Bag needed to
            return this.View(viewModel);
        }

        private DateTime SetStartingDate(int startOfWeek)
        {
            DateTime today = DateTime.Now;
            int daysIntoTheWeek = (int)today.DayOfWeek < startOfWeek
                ? (int)today.DayOfWeek + (7 - startOfWeek)
                : (int)today.DayOfWeek - startOfWeek;
            return today.AddDays(-daysIntoTheWeek);
        }

        /// <summary>
        /// Constuct index view Model for
        /// </summary>
        /// <returns></returns>
        public AccountIndexViewModel ConstuctIndexViewModel()
        {
            User accountInfo = AppService.GetCurrentUser();


            AccountIndexViewModel.UserViewModel userViewModel = new AccountIndexViewModel.UserViewModel(accountInfo);
            AccountIndexViewModel indexViewModel = new AccountIndexViewModel()
            {
                UserInfo = userViewModel
            };
            if (accountInfo == null)
            {
                //if not signed in do not attpempt to load values.
                return indexViewModel;
            }

            //Add invitations to view model

            var invitationslist = accountInfo.Invitations;
            foreach (var inviteInfo in invitationslist)
            {
                var invite = inviteInfo.invite;

                indexViewModel.Invitations.Add(new AccountIndexViewModel.InvitationViewModel()
                {
                    InvitationId = invite.InvitationId,
                    OrganizationName = inviteInfo.invitingOrgName,
                });
            }

            var orgs = accountInfo.Organizations;
            //Add organizations to model
            foreach (var curorg in orgs)
            {
                AccountIndexViewModel.OrganizationViewModel orgViewModel =
                new AccountIndexViewModel.OrganizationViewModel(curorg.Organization,
                    this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, curorg.Organization.OrganizationId, curorg.OrganizationRole, false));

               
                //Add subscription info
                foreach (UserSubscription userSubInfo in accountInfo.Subscriptions.Where(sub => sub.Subscription.OrganizationId == curorg.Organization.OrganizationId))
                {
                    string description =
                        userSubInfo.Subscription.ProductId == ProductIdEnum.TimeTracker ? Resources.Strings.TimeTrackerDescription :
                        userSubInfo.Subscription.ProductId == ProductIdEnum.ExpenseTracker ? Resources.Strings.ExpenseTrackerDescription :
                        "";
                    var subViewModel = new AccountIndexViewModel.OrganizationViewModel.SubscriptionViewModel(userSubInfo, description);
                    if (userSubInfo.Subscription.ProductId == ProductIdEnum.TimeTracker)
                    {
                        int? sDate = null;
                        int? eDate = null;
                        int startOfWeek = AppService.GetAllSettings(userSubInfo.Subscription.SubscriptionId).Item1.StartOfWeek;
                        sDate = AppService.GetDayFromDateTime(SetStartingDate(startOfWeek));
                        eDate = AppService.GetDayFromDateTime(SetStartingDate(startOfWeek).AddDays(6));
                        subViewModel.ProductGoToUrl = Url.RouteUrl("TimeTracker_NoUserId", new
                        {
                            subscriptionId =
                            userSubInfo.Subscription.SubscriptionId,
                            controller = ControllerConstants.TimeEntry,
                            startDate = sDate,
                            endDate = eDate
                        });
                    }
                    else
                    {
                        subViewModel.ProductGoToUrl = userSubInfo.Subscription.AreaUrl;
                    }
                    subViewModel.IconUrl = string.Format("Content/icons/{0}.png", subViewModel.ProductName.Replace(" ", ""));
                    orgViewModel.Subscriptions.Add(subViewModel);
                }
                indexViewModel.Organizations.Add(orgViewModel);
            }

            return indexViewModel;
        }
    }
}
