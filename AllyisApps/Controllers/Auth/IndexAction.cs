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
            var infos = AppService.GetUserOrgsAndInvitationInfo();
            User user = infos.Item1;
            Services.Lookup.Address userAddress = infos.Item4;

            AccountIndexViewModel.UserViewModel userViewModel = new AccountIndexViewModel.UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhoneExtension = user.PhoneExtension,
                Address1 = userAddress.Address1,
                Address2 = userAddress.Address2,
                City = userAddress.City,
                State = userAddress.StateName,
                PostalCode = userAddress.PostalCode,
                Country = userAddress.CountryName,
            };
            AccountIndexViewModel indexViewModel = new AccountIndexViewModel()
            {
                UserInfo = userViewModel
            };
            //Add invitations to view model
            List<InvitationInfo> invitationslist = infos.Item3;
            foreach (InvitationInfo invite in invitationslist)
            {
                var org = AppService.GetOrganization(invite.OrganizationId);

                indexViewModel.Invitations.Add(new AccountIndexViewModel.InvitationViewModel()
                {
                    InvitationId = invite.InvitationId,
                    OrganizationName = org.OrganizationName,
                    OrganizationId = org.OrganizationId
                });
            }

            List<Organization> orgs = infos.Item2;
            //Add organizations to model
            foreach (Organization curorg in orgs)
            {

                AccountIndexViewModel.OrganizationViewModel orgViewModel =
                new AccountIndexViewModel.OrganizationViewModel(curorg, this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, curorg.OrganizationId, false))
                {
                    //TODO: Infomation is dependent on curent user
                    IsManageAllowed = this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, curorg.OrganizationId, false),
                };


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
                            orgViewModel.Subscriptions.Add(new AccountIndexViewModel.OrganizationViewModel.TimeTrackerSubViewModel()
                            {
                                ProductName = userSubInfo.ProductName,
                                SubscriptionId = userSubInfo.SubscriptionId,
                                SubscriptionName = userSubInfo.SubscriptionName,
                                ProductDescription = description,
                                productID = userSubInfo.ProductId,
                                AreaUrl = userSubInfo.AreaUrl,
                                startDate = sDate,
                                endDate = eDate
                            } );
                        }
                        else { 
                        orgViewModel.Subscriptions.Add(new AccountIndexViewModel.OrganizationViewModel.SubscriptionViewModel()
                            {
                                ProductName = userSubInfo.ProductName,
                                SubscriptionId = userSubInfo.SubscriptionId,
                                SubscriptionName = userSubInfo.SubscriptionName,
                                ProductDescription = description,
                                productID = userSubInfo.ProductId,
                                AreaUrl = userSubInfo.AreaUrl,
                            });
                        }
                    }
                }

                indexViewModel.Organizations.Add(orgViewModel);
            }

            return indexViewModel;
        }
    }
}
