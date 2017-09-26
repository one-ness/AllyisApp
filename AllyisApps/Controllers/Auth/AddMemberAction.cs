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
		/// <param name="add">The View Model of user info passed from Add.cshtml.</param>
		/// <param name="organizationId">Organization id.</param>
		/// <returns>The result of this action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddMember(AddMemberViewModel add, int organizationId)
		{
			if (ModelState.IsValid)
			{
				this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, add.OrganizationId);

				try
				{
					Invitation info = new Invitation
					{
						Email = add.Email.Trim(),
						FirstName = add.FirstName,
						LastName = add.LastName,
						OrganizationId = add.OrganizationId,
						OrganizationRole = add.AddAsOwner ? OrganizationRole.Owner : OrganizationRole.Member,
						EmployeeId = add.EmployeeId,
					};

					User usr = AppService.GetUserByEmail(info.Email);
					string url = usr != null && usr.Email == info.Email ?
						Url.Action(ActionConstants.Index, ControllerConstants.Account, null, protocol: Request.Url.Scheme) :
						Url.Action(ActionConstants.Register, ControllerConstants.Account, null, protocol: Request.Url.Scheme);

					int invitationId = AppService.InviteUser(url, info);

					Notifications.Add(new BootstrapAlert(string.Format("{0} {1} " + Resources.Strings.UserEmailed, add.FirstName, add.LastName), Variety.Success));
					return this.RedirectToAction(ActionConstants.ManageOrg, new { id = add.OrganizationId });
				}
				catch (ArgumentException ex)
				{
					if (ex.ParamName.Equals("invitationInfo.Email"))
					{
						Notifications.Add(new BootstrapAlert(Resources.Strings.InvalidEmail, Variety.Danger));
						return this.View(add);
					}

					throw ex;
				}
				catch (InvalidOperationException)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.EmployeeIdNotUniqueError, Variety.Danger));
					return this.View(add);
				}
				catch (System.Data.DuplicateNameException)
				{
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1} " + Resources.Strings.UserAlreadyExists, add.FirstName, add.LastName), Variety.Warning));
					return this.View(add);
				}
			}

			// Invalid model; try again
			return this.View(add);
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