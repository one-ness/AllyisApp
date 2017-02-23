﻿//------------------------------------------------------------------------------
// <copyright file="AddAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
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
		/// GET: /Add.
		/// The page for adding members to an organization.
		/// </summary>
		/// <param name="returnUrl">The return url to redirect to after form submit.</param>
		/// <returns>The result of this action.</returns>
		public ActionResult Add(string returnUrl)
		{
			// Only owners should view this page
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				AddMemberViewModel model = ConstructOrganizationAddMembersViewModel();

				ViewBag.returnUrl = returnUrl;
				return this.View(model);
			}

			ViewBag.ErrorInfo = "Permission";
			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.CannotEditMembersMessage), ControllerConstants.Account, ActionConstants.Add));
		}

		/// <summary>
		/// Uses services to populate the lists of an <see cref="AddMemberViewModel"/> and returns it.
		/// </summary>
		/// <returns>The OrganizationAddMembersViewModel.</returns>
		public AddMemberViewModel ConstructOrganizationAddMembersViewModel()
		{
			var infos = Service.GetAddMemberInfo();

			AddMemberViewModel result = new AddMemberViewModel
			{
				RecommendedEmployeeId = infos.Item1,
				Subscriptions = new List<AddMemberSubscriptionInfo>(),
				Projects = infos.Item4
			};

			foreach(SubscriptionDisplayInfo sub in infos.Item2)
			{
				AddMemberSubscriptionInfo subInfo = new AddMemberSubscriptionInfo
				{
					ProductName = sub.ProductName,
					ProductRoles = infos.Item3.Where(r => r.ProductId == sub.ProductId).ToList(),
					SubscriptionId = sub.SubscriptionId,
					hasTooManySubscribers = sub.SubscriptionsUsed >= sub.NumberOfUsers
				};
				subInfo.ProductRoles.Insert(0, new SubscriptionRoleInfo
				{
					Name = "None",
					ProductId = (int)ProductIdEnum.None,
					ProductRoleId = (int)ProductRole.NotInProduct
				});
				result.Subscriptions.Add(subInfo);
			}

			//OrganizationAddMembersViewModel result = new OrganizationAddMembersViewModel
			//{
			//	Organization = Service.GetOrganization(UserContext.ChosenOrganizationId),
			//	OrganizationId = UserContext.ChosenOrganizationId,
			//	OrganizationProjects = Service.GetProjectsByOrganization(UserContext.ChosenOrganizationId),
			//	EmployeeId = Service.GetRecommendedEmployeeId()
			//};

			//List<SubscriptionRoleSelectionModel> roles = new List<SubscriptionRoleSelectionModel>();
			//IEnumerable<InvitationSubRoleInfo> invitedSubs = Service.GetInvitationSubRoles();
			//IEnumerable<SubscriptionDisplayInfo> subscriptions = Service.GetSubscriptionsDisplay();

			//foreach (SubscriptionDisplayInfo subscription in subscriptions)
			//{
			//	List<SubscriptionRoleInfo> subRoles = Service.GetProductRolesFromSubscription(subscription.SubscriptionId).ToList();
			//	subRoles.Insert(
			//		0,
			//		new SubscriptionRoleInfo
			//		{
			//			Name = "None",
			//			ProductRoleId = (int)ProductRole.NotInProduct
			//		});
			//	roles.Add(
			//		new SubscriptionRoleSelectionModel
			//		{
			//			SubscriptionId = subscription.SubscriptionId,
			//			ProductName = subscription.ProductName,
			//			Roles = subRoles,
			//			Disabled = subscription.NumberOfUsers <= subscription.SubscriptionsUsed + invitedSubs.Where(i => i.SubscriptionId == subscription.SubscriptionId).Count()
			//		});
			//}

			//result.SubscriptionRoles = roles;

			return result;
		}
	}
}
