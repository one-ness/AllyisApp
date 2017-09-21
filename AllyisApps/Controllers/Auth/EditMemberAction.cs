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
using AllyisApps.ViewModels.Auth;

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
				OrganizationUser userOrgInfo = AppService.GetOrganizationManagementInfo(orgId).Users.Find(m => m.UserId == userId);
				User userBasicInfo = AppService.GetUser(userId);

				model = new EditMemberViewModel
				{
					UserInfo = new UserInfoViewModel()
					{
						UserId = userBasicInfo.UserId,
						DateOfBirth = userBasicInfo.DateOfBirth,
						PhoneNumber = userBasicInfo.PhoneNumber,
						PhoneExtension = userBasicInfo.PhoneExtension,
						Email = userBasicInfo.Email,
						FirstName = userBasicInfo.FirstName,
						LastName = userBasicInfo.LastName,

						Address = userBasicInfo.Address?.Address1,
						City = userBasicInfo.Address?.City,
						CountryName = userBasicInfo.Address?.CountryName,
						PostalCode = userBasicInfo.Address?.PostalCode,
						StateName = userBasicInfo.Address?.StateName,
					},
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
				Invitation userOrgInfo = AppService.GetOrganizationManagementInfo(orgId).Invitations.Find(m => m.InvitationId == userId);

				model = new EditMemberViewModel
				{
					UserInfo = new UserInfoViewModel
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
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditMember(EditMemberViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (this.AppService.UpdateMember(model.UserInfo.UserId, model.OrganizationId, model.EmployeeId, model.EmployeeRoleId, model.FirstName, model.LastName, model.IsInvited))
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
