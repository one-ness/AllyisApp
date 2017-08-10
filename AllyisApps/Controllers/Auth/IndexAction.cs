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
using AllyisApps.Services.Lookup;

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
            User user = AppService.GetCurrentUserProfile();
            Services.Lookup.Address userAddress = user.Address;

            AccountIndexViewModel.UserViewModel userViewModel = new AccountIndexViewModel.UserViewModel(user);
            AccountIndexViewModel indexViewModel = new AccountIndexViewModel()
            {
                UserInfo = userViewModel
            };
            //Add invitations to view model
            List<InvitationInfo> invitationslist = AppService.GetInvitationsByUser(user.Email);
            foreach (InvitationInfo invite in invitationslist)
            {
                var org = AppService.GetOrganization(invite.OrganizationId);

                indexViewModel.Invitations.Add(new AccountIndexViewModel.InvitationViewModel()
                {
                    InvitationId = invite.InvitationId,
                    OrganizationName = org.OrganizationName,
                    //OrganizationId = org.OrganizationId
                });
            }

            List<Organization> orgs = AppService.GetOrganizationsByUserId(user.UserId).ToList();
            //Add organizations to model
            foreach (Organization curorg in orgs)
            {

                AccountIndexViewModel.OrganizationViewModel orgViewModel =
                new AccountIndexViewModel.OrganizationViewModel(curorg, this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, curorg.OrganizationId, false));


                //Get orgs Subscriptions
                UserOrganization userOrgInfo;
                AppService.UserContext.UserOrganizations.TryGetValue(curorg.OrganizationId, out userOrgInfo);

                if (userOrgInfo != null)
                {
                    //Add subscription info 
                    foreach (UserSubscription userSubInfo in userOrgInfo.OrganizationSubscriptions.Values)
                    {
                        string description =
                            userSubInfo.ProductId == ProductIdEnum.TimeTracker ? Resources.Strings.TimeTrackerDescription :
                            userSubInfo.ProductId == ProductIdEnum.ExpenseTracker ? Resources.Strings.ExpenseTrackerDescription :
                            "";
                        int? sDate = null;
                        int? eDate = null;
                        if (userSubInfo.ProductId == ProductIdEnum.TimeTracker)
                        {
                            int startOfWeek = AppService.GetAllSettings(userSubInfo.SubscriptionId).Item1.StartOfWeek;
                            sDate = AppService.GetDayFromDateTime(SetStartingDate(startOfWeek));
                            eDate = AppService.GetDayFromDateTime(SetStartingDate(startOfWeek).AddDays(6));
                            //orgViewModel.Subscriptions.Add(
                            //    new AccountIndexViewModel.OrganizationViewModel.TimeTrackerSubViewModel
                            //    (userSubInfo,description,sDate,eDate));
                        }
                        else {
                            orgViewModel.Subscriptions.Add(
                                new AccountIndexViewModel.OrganizationViewModel.SubscriptionViewModel(userSubInfo, description));
                        }
                    }
                }

                indexViewModel.Organizations.Add(orgViewModel);
            }

            return indexViewModel;
        }
    }
}
