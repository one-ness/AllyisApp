﻿//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using AllyisApps.ViewModels.Shared;

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
			UserInfoViewModel model = new UserInfoViewModel
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
			model.UserInfo.Email = Service.GetCompressedEmail(model.UserInfo.Email);

			foreach (SubscriptionDisplayInfo row in model.Subscriptions.Subscriptions)
			{
				DateTime date = Service.GetDateAddedToSubscriptionByUserId(row.SubscriptionId);
				row.CreatedUTC = date;
			}

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