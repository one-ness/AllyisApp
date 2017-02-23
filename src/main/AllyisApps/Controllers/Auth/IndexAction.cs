//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using AllyisApps.ViewModels.Shared;
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
			UserInfoViewModel accountModel = new UserInfoViewModel
			{
				UserInfo = Service.GetUserInfo(),
				Subscriptions = new SubscriptionsViewModel
				{
					Subscriptions = Service.GetUserSubscriptionOrganizationList(),
					ProductList = Service.GetProductInfoList()
				},
				Invitations = Service.GetInvitationsByUser(UserContext.Email).Select(x => new InvitationViewModel
				{
					InvitationId = x.InvitationId,
					OrganizationId = x.OrganizationId,
					OrganizationName = Service.GetOrganization(x.OrganizationId).Name,
				}).ToList()
			};

			foreach (SubscriptionDisplayInfo sub in accountModel.Subscriptions.Subscriptions)
			{
				if (sub.ProductId == (int)ProductIdEnum.TimeTracker)
				{
					sub.CanViewSubscription = Service.Can(Actions.CoreAction.TimeTrackerEditSelf, false, sub.OrganizationId) || Service.Can(Actions.CoreAction.TimeTrackerEditOthers, false, sub.OrganizationId);
				}
			}

			accountModel.UserInfo.Email = Service.GetCompressedEmail(accountModel.UserInfo.Email);

			foreach (SubscriptionDisplayInfo row in accountModel.Subscriptions.Subscriptions)
			{
				DateTime date = Service.GetDateAddedToSubscriptionByUserId(row.SubscriptionId);
				row.CreatedUTC = date;
			}

			ViewBag.ShowOrganizationPartial = false;

			// Old OrganizationsAction
			List<SubscriptionsViewModel> modelList = new List<SubscriptionsViewModel>();

			IEnumerable<OrganizationInfo> orgs = Service.GetOrganizationsByUserId();
			List<ProductInfo> productList = Service.GetProductInfoList();
			foreach (OrganizationInfo org in orgs)
			{
				modelList.Add(new SubscriptionsViewModel
				{
					Subscriptions = Service.GetSubscriptionsDisplay(org.OrganizationId),
					ProductList = productList,
					OrgInfo = org,
					CanEditOrganization = Service.Can(Actions.CoreAction.EditOrganization, false, org.OrganizationId),
					TimeTrackerViewSelf = Service.Can(Actions.CoreAction.TimeTrackerEditSelf, false, org.OrganizationId)
				});
			}

			foreach (SubscriptionsViewModel subVM in modelList)
			{
				foreach (SubscriptionDisplayInfo sub in subVM.Subscriptions)
				{
					if (sub.ProductId == (int)ProductIdEnum.TimeTracker)
					{
						sub.CanViewSubscription = Service.Can(Actions.CoreAction.TimeTrackerEditSelf, false, sub.OrganizationId) || Service.Can(Actions.CoreAction.TimeTrackerEditOthers, false, sub.OrganizationId);
					}
				}
			}

			AccountOrgsViewModel orgmodel = new AccountOrgsViewModel
			{
				Organizations = modelList
			};

			ViewBag.ShowOrganizationPartial = false;


			
			var infos = Service.GetUserOrgsAndInvitationInfo();

			IndexAndOrgsViewModel model = new IndexAndOrgsViewModel
			{
				UserModel = accountModel,
				OrgModel = orgmodel,
				UserInfo = infos.Item1,
				OrgInfos = infos.Item2,
				InviteInfos = infos.Item3
			};
			
			return this.View(model);
		}

		/// <summary>
		/// Action that accepts an invitation to an organization.
		/// </summary>
		/// <param name="invitationId">The id of the accepted invitation.</param>
		/// <returns>The action result.</returns>
		[HttpPost]
		public ActionResult Accept(int invitationId)
		{
			var invitation = Service.GetInvitationsByUser(UserContext.Email).Where(x => x.InvitationId == invitationId).FirstOrDefault();
			if (invitation != null)
			{
				// Validate that the user does have the requested pending invitation
				Notifications.Add(new Core.Alert.BootstrapAlert(
					Service.AcceptUserInvitation(invitation), Core.Alert.Variety.Success));
			}
			else
			{
				// Not a part of the invitation
				Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			}

			return RedirectToAction(ActionConstants.Index);
		}

		/// <summary>
		/// Action that rejects an invitation to an organization.
		/// </summary>
		/// <param name="invitationId">The id of the accepted invitation.</param>
		/// <returns>The Action result.</returns>
		[HttpPost]
		public ActionResult Reject(int invitationId)
		{
			var invitation = Service.GetInvitationsByUser(UserContext.Email).Where(x => x.InvitationId == invitationId).FirstOrDefault();
			if (invitation != null)
			{
				// Validate that the user does have the requested pending invitation
				Notifications.Add(new Core.Alert.BootstrapAlert(Service.RejectUserInvitation(invitation), Core.Alert.Variety.Success));
			}
			else
			{
				// Not a part of the invitation
				Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			}

			return RedirectToAction(ActionConstants.Index);
		}
	}
}
