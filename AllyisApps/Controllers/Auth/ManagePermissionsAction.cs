//------------------------------------------------------------------------------
// <copyright file="ManagePermissionsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using Newtonsoft.Json;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Dictionaries of Strings resource used for view Maybe: Move to view Model
		/// </summary>
		private Dictionary<int, string> organizationRoles = new Dictionary<int, string>
		{
		{ (int)OrganizationRoleEnum.Member, Strings.Member },
		{ (int)OrganizationRoleEnum.Owner, Strings.Owner }
		};

		private Dictionary<int, string> ttRoles = new Dictionary<int, string>
		{
			{ (int)TimeTrackerRole.User, Strings.User },
			{ (int)TimeTrackerRole.Manager, Strings.Manager },
			{ (int)TimeTrackerRole.NotInProduct, Strings.Unassigned }
		};

		private Dictionary<int, string> etRoles = new Dictionary<int, string>
	{
		{ (int)ExpenseTrackerRole.User, Strings.User },
		{ (int)ExpenseTrackerRole.Manager, Strings.Manager },
		{ (int)ExpenseTrackerRole.SuperUser, "Super User" },
		{ (int)ExpenseTrackerRole.NotInProduct, Strings.Unassigned }
	};

		private Dictionary<string, int> setOrganizationRoles = new Dictionary<string, int>
	{
		{ Strings.RemoveOrg, -1 },
		{ Strings.SetMember, (int)OrganizationRoleEnum.Member },
		{ Strings.SetOwner, (int)OrganizationRoleEnum.Owner }
	};

		private Dictionary<string, int> setTTRoles = new Dictionary<string, int>
	{
		{ Strings.RemoveFromSubscription, -1 },
		{ Strings.SetUser, (int)TimeTrackerRole.User },
		{ Strings.SetManager, (int)TimeTrackerRole.Manager }
	};

		private Dictionary<string, int> setETRoles = new Dictionary<string, int>
	{
		{ Strings.RemoveFromSubscription, -1},
		{ Strings.SetUser, (int)ExpenseTrackerRole.User },
		{ Strings.SetManager, (int)ExpenseTrackerRole.Manager },
		{ "Set Super User", (int)ExpenseTrackerRole.SuperUser }
	};

		/// <summary>
		///
		/// </summary>
		/// <param name="id">Organizaion Id.</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> ManageOrgPermissions(int id)
		{
			//Get OrganizaionUser Rows
			AppService.CheckOrgAction(AppService.OrgAction.EditUserPermission, id);
			var orgUsers = AppService.GetOrganizationMemberList(id);
			var orgSubs = AppService.GetSubscriptionsByOrg(id);

			PermissionsViewModel perModel = new PermissionsViewModel()
			{
				Actions = setOrganizationRoles,
				ActionGroup = Strings.Organization,
				PossibleRoles = organizationRoles,
				RemoveUserMessage = Strings.RemoveFromOrgNoName,
				RoleHeader = Strings.OrganizationRole,
				CurrentSubscriptions = orgSubs.Select(sub => new PermissionsViewModel.OrganizaionSubscriptionsViewModel()
				{
					ProductId = (int)sub.ProductId,
					ProductName = sub.ProductName,
					SubscriptionId = sub.SubscriptionId,
					SubscriptionName = sub.SubscriptionName
				}).OrderBy(sub => sub.ProductId).ToList(),

				OrganizationId = id,
				ProductId = null,
				SubscriptionId = null,
				Users = orgUsers.Select(orgU => new UserPermssionViewModel()
				{
					currentRole = orgU.OrganizationRoleId,
					CurrentRoleName = organizationRoles[orgU.OrganizationRoleId],
					Email = orgU.Email,
					FullName = orgU.FirstName + " " + orgU.LastName,
					UserId = orgU.UserId,
					isChecked = false
				}).OrderBy(orgU => orgU.FullName).ToList()
			};

			await Task.Delay(1);
			return View("PermissionsOrg", perModel);
		}

		/// <summary>
		/// Get page to edit SubscriptionPermissions
		/// </summary>
		/// <param name="id">Subscription ID</param>
		/// <returns></returns>
		[HttpGet]
		public async Task<ActionResult> ManageSubPermissions(int id)
		{
			var sub = await AppService.GetSubscription(id);
			var orgSubs = AppService.GetSubscriptionsByOrg(sub.OrganizationId);

			var subUsers = AppService.GetSubscriptionUsers(id);
			var organizationMembers = AppService.GetOrganizationMemberList(sub.OrganizationId);

			//Get Strings speffic to Product for page
			Dictionary<int, string> roles = null;
			Dictionary<string, int> actions = null;
			String roleHeader = null;
			String ActionGroup = null;
			switch (sub.ProductId)
			{
				case ProductIdEnum.TimeTracker:
					roles = ttRoles;
					actions = setTTRoles;
					roleHeader = Strings.TimeTrackerRole;
					ActionGroup = Strings.TimeTracker;
					break;

				case ProductIdEnum.ExpenseTracker:
					roles = etRoles;
					actions = setETRoles;
					roleHeader = Strings.ExpenseTrackerRole;
					ActionGroup = Strings.ExpenseTracker;
					break;

				case ProductIdEnum.StaffingManager:
					throw new NotImplementedException("StaffingManager permissions not implmented");
			}

			List<UserPermssionViewModel> OrgUsers = organizationMembers.Select(orgU => new UserPermssionViewModel()
			{
				currentRole = (int)ProductRole.NotInProduct,
				CurrentRoleName = Strings.Unassigned,
				FullName = orgU.FirstName + " " + orgU.LastName,
				Email = orgU.Email,
				isChecked = false,
				UserId = orgU.UserId
			}).OrderBy(orgU => orgU.FullName).ToList();

			foreach (var subU in subUsers)
			{
				var OrgUserWithSub = OrgUsers.First(orgU => orgU.UserId == subU.UserId);
				OrgUserWithSub.currentRole = subU.ProductRoleId;
				OrgUserWithSub.CurrentRoleName = roles[subU.ProductRoleId];
			}

			PermissionsViewModel model = new PermissionsViewModel()
			{
				Actions = actions,
				OrganizationId = sub.OrganizationId,
				ActionGroup = ActionGroup,
				PossibleRoles = roles,
				ProductId = (int)sub.ProductId,
				CurrentSubscriptions = orgSubs.Select(cursub => new PermissionsViewModel.OrganizaionSubscriptionsViewModel()
				{
					ProductId = (int)cursub.ProductId,
					ProductName = cursub.ProductName,
					SubscriptionId = cursub.SubscriptionId,
					SubscriptionName = cursub.SubscriptionName
				}).OrderBy(cursub => cursub.ProductId).ToList(),
				SubscriptionId = id,
				RoleHeader = roleHeader,
				RemoveUserMessage = "Are you sure you want to remove selcted Users from Subscription",
				Users = OrgUsers
			};

			await Task.Delay(1);
			return View("PermissionsOrg", model);
		}
	}
}