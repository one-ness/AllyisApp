//------------------------------------------------------------------------------
// <copyright file="AddMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;
using System.Collections.Generic;
using AllyisApps.Services.Billing;
using AllyisApps.Resources;

namespace AllyisApps.Controllers.Auth
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
		public async Task<ActionResult> AddMember(int id)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.AddUserToOrganization, id);
			var model = new AddMemberViewModel();
			model.OrganizationId = id;
			model.EmployeeId = await this.AppService.GetNextEmployeeId(id);

			var etSubInfo = AppService.GetProductSubscriptionInfo(id, SkuIdEnum.ExpenseTrackerBasic).SubscriptionInfo;
			var ttSubInfo = AppService.GetProductSubscriptionInfo(id, SkuIdEnum.TimeTrackerBasic).SubscriptionInfo;

			model.hasET = etSubInfo != null ? true : false;
			model.hasTT = ttSubInfo != null ? true : false;

			List<SelectListItem> orgRoles = new List<SelectListItem>()
			{
				new SelectListItem() { Text = OrganizationRole.Member.ToString(), Value = "1"},
				new SelectListItem() { Text = OrganizationRole.Owner.ToString(), Value = "2" }
			};

			List<SelectListItem> etRoles = new List<SelectListItem>()
			{
				new SelectListItem() { Text = Strings.Unassigned, Value = "0"},
				new SelectListItem() { Text = Strings.User, Value = "1"},
				new SelectListItem() { Text = Strings.Manager, Value = "2"},
				new SelectListItem() { Text = Strings.SuperUser, Value = "4"},
			};

			List<SelectListItem> ttRoles = new List<SelectListItem>()
			{
				new SelectListItem() { Text = Strings.Unassigned, Value = "0"},
				new SelectListItem() { Text = Strings.User, Value = "1"},
				new SelectListItem() { Text = Strings.Manager, Value = "2"}
			};

			model.OrgRole = new SelectList(orgRoles, "Value", "Text", "1");
			model.TTRoles = new SelectList(ttRoles, "Value", "Text", "0");
			model.ETRoles = new SelectList(etRoles, "Value", "Text", "0");

			return this.View(model);
		}

		/// <summary>
		/// POST: /Add
		/// Adding a new member to an organization.
		/// </summary>
		/// <param name="model">The View Model of user info passed from Add.cshtml.</param>
		/// <returns>The result of this action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		async public Task<ActionResult> AddMember(AddMemberViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					User usr = await AppService.GetUserByEmail(model.Email);
					string url = usr != null ?
						Url.Action(ActionConstants.Index, ControllerConstants.Account, null, protocol: Request.Url.Scheme) :
						Url.Action(ActionConstants.Register, ControllerConstants.Account, null, protocol: Request.Url.Scheme);

					string prodJson = string.Format("{{ \"" + (int)SkuIdEnum.TimeTrackerBasic + "\" : {0}, \"" + (int)SkuIdEnum.ExpenseTrackerBasic + "\" : {1}, \"" + (int)SkuIdEnum.StaffingManagerBasic + "\" : 0 }}", model.ttSelection, model.etSelection);

					int invitationId = await AppService.InviteUser(url, model.Email.Trim(), model.FirstName, model.LastName, model.OrganizationId, model.OrgRoleSelection == 2 ? OrganizationRole.Owner : OrganizationRole.Member, model.EmployeeId, prodJson);

					Notifications.Add(new BootstrapAlert(string.Format("{0} {1} " + Resources.Strings.UserEmailed, model.FirstName, model.LastName), Variety.Success));
					return this.RedirectToAction(ActionConstants.OrganizationMembers, new { id = model.OrganizationId });
				}
				catch (InvalidOperationException)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.EmployeeIdNotUniqueError, Variety.Danger));
					return this.View(model);
				}
				catch (System.Data.DuplicateNameException)
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1} " + Resources.Strings.UserAlreadyExists, model.FirstName, model.LastName), Variety.Warning));
					return this.View(model);
				}
			}

			// Invalid model; try again
			return this.View(model);
		}
	}
}