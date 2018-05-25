//------------------------------------------------------------------------------
// <copyright file="AddMemberAction.cs" company="Allyis, Inc.">
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
			AddMemberViewModel model = await constuctViewModel(id);

			return View(model);
		}

		private async Task<AddMemberViewModel> constuctViewModel(int id)
		{
			var subs = await AppService.GetSubscriptionsAsync(id);

			var employeeTypes = await AppService.GetEmployeeTypeByOrganization(id);
			var employeeTypeViewModels = new List<EmployeeTypeViewModel>();
			foreach (var employeeType in employeeTypes)
			{
				employeeTypeViewModels.Add(new EmployeeTypeViewModel()
				{
					EmployeeTypeId = employeeType.EmployeeTypeId,
					EmployeeTypeName = employeeType.EmployeeTypeName,
					OrganizationId = employeeType.OrganizationId
				});
			}

			var model = new AddMemberViewModel
			{
				OrganizationId = id,
				EmployeeTypeList = employeeTypeViewModels,
				EmployeeId = await AppService.GetNextEmployeeId(id),
				OrganizationName = "Organization", //AppService.GetOrganization(id).Result.OrganizationName;
				SubscriptionRoles = subs.Select(sub => new RoleItem
				{
					ProductId = (int)sub.ProductId,
					SubscriptionName = sub.SubscriptionName,
					SelectList = GetSubRoles(sub.SkuId),
					SubscriptionId = sub.SubscriptionId,
				}).ToList(),
				OrgRole = new SelectList(
					new List<SelectListItem>
					{
						new SelectListItem { Text = OrganizationRoleEnum.Member.GetEnumName(), Value = ((int)OrganizationRoleEnum.Member).ToString()},
						new SelectListItem { Text = OrganizationRoleEnum.Admin.GetEnumName(), Value = ((int)OrganizationRoleEnum.Member).ToString()}
					},
					"Value",
					"Text",
					"1")
			};

			for (int i = 0; i < model.SubscriptionRoles.Count(); i++)
			{
				List<SelectListItem> roleList = new List<SelectListItem>();
				foreach (var role in model.SubscriptionRoles[i].SelectList)
				{
					roleList.Add(new SelectListItem()
					{
						Value = role.Value,
						Text = role.Text
					});
				}
				model.SubscriptionRoles[i].SelectList = roleList;
			}
			return model;
		}

		private async Task<AddMemberViewModel> reconstuctViewModel(AddMemberViewModel previous)
		{
			var subs = await AppService.GetSubscriptionsAsync(previous.OrganizationId);
			previous.SubscriptionRoles = subs.Select(sub => new RoleItem
			{
				ProductId = (int)sub.ProductId,
				SubscriptionName = sub.SubscriptionName,
				SelectList = GetSubRoles(sub.SkuId),
				SubscriptionId = sub.SubscriptionId,
			}).ToList();
			previous.OrgRole = new SelectList(
					new List<SelectListItem>
					{
						new SelectListItem { Text = OrganizationRoleEnum.Member.GetEnumName(), Value = ((int)OrganizationRoleEnum.Member).ToString()},
						new SelectListItem { Text = OrganizationRoleEnum.Admin.GetEnumName(), Value = ((int)OrganizationRoleEnum.Member).ToString()}
					},
					"Value",
					"Text",
					"1");
			return previous;
		}

		private static List<SelectListItem> GetSubRoles(SkuIdEnum skuId)
		{
			switch (skuId)
			{
				case SkuIdEnum.TimeTrackerBasic:
					return new List<SelectListItem>
					{
						new SelectListItem { Text = TimeTrackerRole.NotInProduct.GetEnumName(), Value = ((int)TimeTrackerRole.NotInProduct).ToString() },
						new SelectListItem { Text = TimeTrackerRole.User.GetEnumName(), Value = ((int)TimeTrackerRole.User).ToString()},
						new SelectListItem { Text = TimeTrackerRole.Admin.GetEnumName(), Value = ((int)TimeTrackerRole.Admin).ToString()}
					};

				case SkuIdEnum.ExpenseTrackerBasic:
					return new List<SelectListItem>
					{
						new SelectListItem { Text = ExpenseTrackerRole.NotInProduct.GetEnumName(), Value = ((int)ExpenseTrackerRole.NotInProduct).ToString()},
						new SelectListItem { Text = ExpenseTrackerRole.User.GetEnumName(), Value = ((int)ExpenseTrackerRole.User).ToString()},
						new SelectListItem { Text = ExpenseTrackerRole.Manager.GetEnumName(), Value = ((int)ExpenseTrackerRole.Manager).ToString()},
						new SelectListItem { Text = ExpenseTrackerRole.Admin.GetEnumName(), Value = ((int)ExpenseTrackerRole.Admin).ToString()},
					};

				case SkuIdEnum.StaffingManagerBasic:
					return new List<SelectListItem>
					{
						new SelectListItem { Text = StaffingManagerRole.NotInProduct.GetEnumName(), Value = ((int)StaffingManagerRole.NotInProduct).ToString()},
						new SelectListItem { Text = StaffingManagerRole.User.GetEnumName(), Value = ((int)StaffingManagerRole.User).ToString()},
						new SelectListItem { Text = StaffingManagerRole.Admin.GetEnumName(), Value = ((int)StaffingManagerRole.Admin).ToString()}
					};
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
			AddMemberViewModel reModel = await constuctViewModel(model.OrganizationId);
			if (!ModelState.IsValid) return View(reModel); // Invalid model, try again

			try
			{
				UserOld usr = await AppService.GetUserOldByEmailAsync(model.Email);
				string redirectLink = usr != null ?
					Url.Action(ActionConstants.Index, ControllerConstants.Account, null, protocol: Request.Url.Scheme) :
					Url.Action(ActionConstants.Register, ControllerConstants.Account, null, protocol: Request.Url.Scheme);
				string url = redirectLink;

				List<InvitationPermissionsJson> json = model.SubscriptionRoles.Select(role => new InvitationPermissionsJson
				{
					SubscriptionId = role.SubscriptionId,
					ProductRoleId = role.SelectedRoleId
				}).ToList<InvitationPermissionsJson>();

				string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(json);
				string orgName = AppService.UserContext.OrganizationsAndRoles[model.OrganizationId].OrganizationName;

				int invitationId = await AppService.InviteUser(
					url,
					model.Email.Trim(),
					model.FirstName,
					model.LastName,
					model.OrganizationId,
					orgName,
					model.OrgRoleSelection == 2 ? OrganizationRoleEnum.Admin : OrganizationRoleEnum.Member,
					model.EmployeeId,
					jsonString,
					model.EmployeeTypeId
					);

				Notifications.Add(new BootstrapAlert(string.Format(Strings.UserEmailed, model.FirstName, model.LastName), Variety.Success));
				return RedirectToAction(ActionConstants.OrganizationInvitations, new { id = model.OrganizationId });
			}
			catch (InvalidOperationException)
			{
				Notifications.Add(new BootstrapAlert(Strings.EmployeeIdNotUniqueError, Variety.Danger));
				return View(reModel);
			}
			catch (System.Data.DuplicateNameException)
			{
				if (AppService.UserContext.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase))
				{
					Notifications.Add(new BootstrapAlert(Strings.CannotInviteSelf, Variety.Warning));
				}
				else
				{
					Notifications.Add(new BootstrapAlert(string.Format(Strings.UserAlreadyExists, model.FirstName, model.LastName), Variety.Warning));
				}
				return View(reModel);
			}
		}
	}
}
