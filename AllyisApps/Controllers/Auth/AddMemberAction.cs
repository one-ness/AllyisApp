//------------------------------------------------------------------------------
// <copyright file="AddMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Resources;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;

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
			AppService.CheckOrgAction(AppService.OrgAction.AddUserToOrganization, id);
			var model = new AddMemberViewModel();
			model.OrganizationId = id;
			model.EmployeeId = await AppService.GetNextEmployeeId(id);
			
			List<SelectListItem> orgRoles = new List<SelectListItem>
			{
				new SelectListItem { Text = OrganizationRoleEnum.Member.ToString(), Value = "1"},
				new SelectListItem { Text = OrganizationRoleEnum.Owner.ToString(), Value = "2" }
			};

			model.OrgRole = new SelectList(orgRoles, "Value", "Text", "1");
			model.SubscriptionRoles = new List<RoleItem>();
			
			var subs = await AppService.GetSubscriptionsAsync(model.OrganizationId);
			foreach (var item in subs)
			{
				model.SubscriptionRoles.Add(new RoleItem()
				{
					ProductId = (int)item.ProductId,
					SubscriptionName = item.SubscriptionName,
					SelectList = GetSubRoles(item.SkuId)
				});
			}

			model.orgName = "Organization"; //AppService.GetOrganization(id).Result.OrganizationName;

			return View(model);
		}

		private List<SelectListItem> GetSubRoles(SkuIdEnum skuId)
		{
			switch (skuId)
			{
				case SkuIdEnum.TimeTrackerBasic:
					return new List<SelectListItem>
					{
						new SelectListItem { Text = Strings.Unassigned, Value = "0"},
						new SelectListItem { Text = Strings.User, Value = "1"},
						new SelectListItem { Text = Strings.Manager, Value = "2"}
					};
				case SkuIdEnum.ExpenseTrackerBasic:
					return new List<SelectListItem>
					{
						new SelectListItem { Text = Strings.Unassigned, Value = "0"},
						new SelectListItem { Text = Strings.User, Value = "1"},
						new SelectListItem { Text = Strings.Manager, Value = "2"},
						new SelectListItem { Text = Strings.SuperUser, Value = "4"},
					};
				case SkuIdEnum.StaffingManagerBasic:
					return new List<SelectListItem>
					{
						new SelectListItem { Text = Strings.Unassigned, Value = "0"},
						new SelectListItem { Text = Strings.User, Value = "1"},
						new SelectListItem { Text = Strings.Manager, Value = "2"}
					};
				default:
					break;
			}
			return null;
		}

		/// <summary>
		/// POST: /Add
		/// Adding a new member to an organization.
		/// </summary>
		/// <param name="model">The View Model of user info passed from Add.cshtml.</param>
		/// <returns>The result of this action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> AddMember(AddMemberViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					User usr = await AppService.GetUserByEmail(model.Email);
					string url = usr != null ?
						Url.Action(ActionConstants.Index, ControllerConstants.Account, null, protocol: Request.Url.Scheme) :
						Url.Action(ActionConstants.Register, ControllerConstants.Account, null, protocol: Request.Url.Scheme);

					string prodJson = "{{ "; //string.Format("{{ \"" + (int)ProductIdEnum.TimeTracker + "\" : {0}, \"" + (int)ProductIdEnum.ExpenseTracker + "\" : {1}, \"" + (int)ProductIdEnum.StaffingManager + "\" : 0 }}", model.ttSelection, model.etSelection);

					foreach (var role in model.SubscriptionRoles)
					{
						prodJson += "\"" + role.ProductId + "\" : " + role.SelectedRoleId + ", ";
					}
					prodJson = prodJson.TrimEnd(new char[] { ' ', ',' });
					prodJson += " }}";

					int invitationId = await AppService.InviteUser(url, model.Email.Trim(), model.FirstName, model.LastName, model.OrganizationId, model.OrgRoleSelection == 2 ? OrganizationRoleEnum.Owner : OrganizationRoleEnum.Member, model.EmployeeId, prodJson);

					Notifications.Add(new BootstrapAlert(string.Format("{0} {1} " + Strings.UserEmailed, model.FirstName, model.LastName), Variety.Success));
					return RedirectToAction(ActionConstants.OrganizationMembers, new { id = model.OrganizationId });
				}
				catch (InvalidOperationException)
				{
					Notifications.Add(new BootstrapAlert(Strings.EmployeeIdNotUniqueError, Variety.Danger));
					return View(model);
				}
				catch (System.Data.DuplicateNameException)
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1} " + Strings.UserAlreadyExists, model.FirstName, model.LastName), Variety.Warning));
					return View(model);
				}
			}

			// Invalid model; try again
			return View(model);
		}
	}
}