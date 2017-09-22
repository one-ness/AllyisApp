//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// GET: /Account/EditMember.
		/// </summary>
		/// <param name="userId">Org member to edit.</param>
		/// <param name="orgId">Id of the org the member is in.</param>
		/// <param name="invited">Is the user invited or a already a member?.</param>
		/// <returns>Returns info for a view about the member to be edited.</returns>
		public ActionResult EditMember(int userId, int orgId, int invited)
		{
			bool isInvited = invited == 0 ? false : true;
			EditMemberViewModel model;
			ViewBag.SignedInUserId = GetCookieData().UserId;

			if (!isInvited)
			{
				OrganizationUserInfo userOrgInfo = AppService.GetOrganizationManagementInfo(orgId).Item2.Find(m => m.UserId == userId);
				User userBasicInfo = AppService.GetUserInfo(userId);

				model = new EditMemberViewModel
				{
					UserInfo = userBasicInfo,
					CurrentUserId = this.AppService.UserContext.UserId,
					FirstName = userBasicInfo.FirstName,
					LastName = userBasicInfo.LastName,
					OrganizationId = orgId,
					EmployeeId = userOrgInfo.EmployeeId,
					EmployeeRoleId = userOrgInfo.OrganizationRoleId,
					IsInvited = isInvited
				};
			}
			else
			{
				InvitationInfo userOrgInfo = AppService.GetOrganizationManagementInfo(orgId).Item4.Find(m => m.InvitationId == userId);

				model = new EditMemberViewModel
				{
					UserInfo = new User
					{
						UserId = userOrgInfo.InvitationId,
						Email = userOrgInfo.Email
					},
					FirstName = userOrgInfo.FirstName,
					LastName = userOrgInfo.LastName,
					OrganizationId = orgId,
					EmployeeId = userOrgInfo.EmployeeId,
					EmployeeRoleId = (int)userOrgInfo.OrganizationRole,
					IsInvited = isInvited
				};
			}

			return View(model);
		}

		/// <summary>
		/// POST: /Account/EditMember.
		/// </summary>
		/// <param name="model">The Edit Member view model, with all the form info that we need to save.</param>
		/// <returns>The async task to redirect to the manage org page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditMember(EditMemberViewModel model)
		{
			Dictionary<string, dynamic> modelData = new Dictionary<string, dynamic>
			{
				{ "employeeId", model.EmployeeId },
				{ "employeeRoleId", model.EmployeeRoleId },
				{ "isInvited", model.IsInvited },
				{ "userId", model.UserInfo.UserId },
				{ "orgId", model.OrganizationId },
				{ "firstName", model.FirstName },
				{ "lastName", model.LastName }
			};

			if (ModelState.IsValid)
			{
				if (await Task.Factory.StartNew(() => AppService.UpdateMember(modelData)))
				{
					Notifications.Add(new BootstrapAlert(string.Format(Resources.Strings.UpdateMemberSuccessMessage, model.UserInfo.FirstName, model.UserInfo.LastName), Variety.Success));
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.CannotEditEmployeeId, Variety.Danger));
				}

				return this.RedirectToAction(ActionConstants.ManageOrg, ControllerConstants.Account, new { id = model.OrganizationId });
			}

			return View(model);
		}
	}
}