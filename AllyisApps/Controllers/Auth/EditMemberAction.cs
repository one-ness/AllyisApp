//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/EditMember.
		/// </summary>
		/// <param name="userId">Org member to edit</param>
		/// <param name="orgId">Id of the org the member is in</param>
		/// <param name="isInvited">Is the user invited or a already a member?</param>
		/// <returns>Returns info for a view about the member to be edited</returns>
		public ActionResult EditMember(int userId, int orgId, bool isInvited)
		{
			OrganizationUserInfo userOrgInfo = AppService.GetOrganizationManagementInfo(orgId).Item2.Find(m => m.UserId == userId);
			ViewBag.SignedInUserID = GetCookieData().UserId;

			EditMemberViewModel model = new EditMemberViewModel
			{
				UserInfo = AppService.GetUser(userId),
				OrganizationId = orgId,
				EmployeeTypeId = userOrgInfo.EmployeeTypeId,
				EmployeeId = userOrgInfo.EmployeeId,
				EmployeeRoleId = userOrgInfo.OrgRoleId,
				IsInvited = isInvited
			};
			return View(model);
		}

		/// <summary>
		/// POST: /Account/EditMember.
		/// </summary>
		/// <param name="model">The Edit Member view model, with all the form info that we need to save</param>
		/// <returns>The async task to redirect to the manage org page</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditMember(EditMemberViewModel model)
		{
			if (ModelState.IsValid)
			{
				await Task.Factory.StartNew(() => AppService.UpdateMember(model.EmployeeId, model.EmployeeTypeId, model.EmployeeRoleId, model.IsInvited, model.UserInfo.UserId, model.OrganizationId));

				Notifications.Add(new BootstrapAlert(String.Format(Resources.Strings.UpdateMemberSuccessMessage, model.UserInfo.FirstName, model.UserInfo.LastName), Variety.Success));

				return this.RedirectToAction(ActionConstants.ManageOrg, ControllerConstants.Account, new { id = model.OrganizationId });
			}

			return View(model);
		}
	}
}