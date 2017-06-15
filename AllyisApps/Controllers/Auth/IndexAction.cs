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
					CanEditOrganization = AppService.Can(Actions.CoreAction.EditOrganization, false, o.OrganizationId),
					Subscriptions = new List<SubscriptionDisplayViewModel>()
				};
				UserOrganizationInfo userOrgInfo = null;
				UserContext.UserOrganizations.TryGetValue(o.OrganizationId, out userOrgInfo);
				if (userOrgInfo != null)
				{
					foreach (UserSubscriptionInfo userSubInfo in userOrgInfo.UserSubscriptions.Values)
					{
						orgVM.Subscriptions.Add(new SubscriptionDisplayViewModel
						{
							ProductId = (int)userSubInfo.ProductId,
							ProductName = userSubInfo.ProductName,
							ProductDisplayName = userSubInfo.ProductId == ProductIdEnum.TimeTracker ? Resources.Strings.TimeTracker : "Unknown Product",
							ProductDescription = userSubInfo.ProductId == ProductIdEnum.TimeTracker ? Resources.Strings.TimeTrackerDescription : ""
						});
					}
				}
				return orgVM;
			}).ToList();

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
			string result = AppService.AcceptUserInvitation(invitationId);

			if (result == null)
			{
				Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			}
			else
			{
				Notifications.Add(new Core.Alert.BootstrapAlert(result, Core.Alert.Variety.Success));
			}

			//var invitation = Service.GetInvitationsByUser(UserContext.Email).Where(x => x.InvitationId == invitationId).FirstOrDefault();
			//if (invitation != null)
			//{
			//	// Validate that the user does have the requested pending invitation
			//	Notifications.Add(new Core.Alert.BootstrapAlert(
			//		Service.AcceptUserInvitation(invitationId), Core.Alert.Variety.Success));
			//}
			//else
			//{
			//	// Not a part of the invitation
			//	Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			//}

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
			//var invitation = Service.GetInvitationsByUser(UserContext.Email).Where(x => x.InvitationId == invitationId).FirstOrDefault();
			string result = AppService.RejectUserInvitation(invitationId);
			if (result != null)
			{
				// Validate that the user does have the requested pending invitation
				Notifications.Add(new Core.Alert.BootstrapAlert(result, Core.Alert.Variety.Success));
			}
			else
			{
				// Not a part of the invitation
				Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			}

			return RedirectToAction(ActionConstants.Index);
		}
	}
}
