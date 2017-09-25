//------------------------------------------------------------------------------
// <copyright file="AddMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
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
		/// <param name="id">Organization id.</param>
		/// <param name="returnUrl">The return url to redirect to after form submit.</param>
		/// <returns>The result of this action.</returns>
		public ActionResult AddMember(int id, string returnUrl)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.AddUserToOrganization, id);
			AddMemberViewModel model = ConstructOrganizationAddMembersViewModel(id);
			ViewBag.returnUrl = returnUrl;
			return this.View(model);
		}

		/// <summary>
		/// POST: /Add
		/// Adding a new member to an organization.
		/// </summary>
		/// <param name="model">The View Model of user info passed from Add.cshtml.</param>
		/// <param name="organizationId">Organization id.</param>
		/// <returns>The result of this action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddMember(AddMemberViewModel model, int organizationId)
		{
			if (ModelState.IsValid)
			{
				try
				{
					User usr = AppService.GetUserByEmail(model.Email);
					string url = usr != null ?
						Url.Action(ActionConstants.Index, ControllerConstants.Account, null, protocol: Request.Url.Scheme) :
						Url.Action(ActionConstants.Register, ControllerConstants.Account, null, protocol: Request.Url.Scheme);

					int invitationId = AppService.InviteUser(url, model.Email.Trim(), model.FirstName, model.LastName, model.OrganizationId, model.AddAsOwner ? OrganizationRole.Owner : OrganizationRole.Member, model.EmployeeId);

					Notifications.Add(new BootstrapAlert(string.Format("{0} {1} " + Resources.Strings.UserEmailed, model.FirstName, model.LastName), Variety.Success));
					return this.RedirectToAction(ActionConstants.ManageOrg, new { id = model.OrganizationId });
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

		/// <summary>
		/// Uses services to populate the lists of an <see cref="AddMemberViewModel"/> and returns it.
		/// </summary>
		/// <param name="organizationId">The Organization Id.</param>
		/// <returns>The OrganizationAddMembersViewModel.</returns>
		public AddMemberViewModel ConstructOrganizationAddMembersViewModel(int organizationId)
		{
			Organization infos = AppService.GetAddMemberInfo(organizationId);

			AddMemberViewModel result = new AddMemberViewModel
			{
				OrganizationId = organizationId,
				EmployeeId = new string(AppService.IncrementAlphanumericCharArray(infos.NextEmpolyeeID.ToCharArray())),
			};

			return result;
		}
	}
}