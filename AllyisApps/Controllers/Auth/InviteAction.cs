//------------------------------------------------------------------------------
// <copyright file="InviteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using System;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		//// TODO: If the services get consolidated into one class, this method could be moved to Services and a business object created to mirror OrganizationAddMembersViewModel.
		////        At the moment, the calls to different service objects would require one having its own reference to or instance of the others in order to move this.

		///// <summary>
		///// Processes an OrganizationAddMembersViewModel as input.
		///// </summary>
		///// <param name="orgAddMembers">The OrganizationAddMembersViewModel to process.</param>
		///// <returns>The OrganizationAddMembersViewModel after processing, with updated AddedUsers/UsersAlreadyExisting/EmailedUsers. Returns null if unsuccessful.</returns>
		//public async Task<OrganizationAddMembersViewModel> ProcessUserInput(OrganizationAddMembersViewModel orgAddMembers)
		//{
		//	if (!string.IsNullOrEmpty(orgAddMembers.Email))
		//	{
		//		string userEmail = orgAddMembers.Email.Trim();
		//		if (Service.IsEmailAddressValid(userEmail))
		//		{ // If input string kinda looks like an email..
		//			User user = Service.GetUserByEmail(userEmail); // ...Attempt to get the user data by email.
		//															   // If that doesn't return null...
		//			if (user != null)
		//			{
		//				OrgRole role = Service.GetOrgRole(orgAddMembers.OrganizationId, user.UserId); // ...see if they have permissions in this organization already
		//																								  // If not...
		//				if (role != null)
		//				{
		//					orgAddMembers.UsersAlreadyExisting.Add(userEmail);
		//					return orgAddMembers;
		//				}
		//			}

		//			// input string is not associated with an existing user
		//			// so send them an email and let them know of the request
		//			orgAddMembers.EmailedUsers.Add(userEmail);
		//			User requestingUser = Service.GetUser();
		//			int code = new Random().Next(100000);
		//			int invitationId = await Service.InviteNewUser(
		//				string.Format("{0} {1}", requestingUser.FirstName, requestingUser.LastName),
		//				GlobalSettings.WebRoot,
		//				new InvitationInfo
		//				{
		//					Email = userEmail,
		//					FirstName = orgAddMembers.FirstName,
		//					LastName = orgAddMembers.LastName,
		//					OrganizationId = orgAddMembers.OrganizationId,
		//					AccessCode = code.ToString(),
		//					DateOfBirth = DateTime.MinValue.AddYears(1754),
		//					OrgRole = (int)(orgAddMembers.AddAsOwner ? OrganizationRole.Owner : OrganizationRole.Member),
		//					ProjectId = orgAddMembers.SubscriptionProjectId,
		//					EmployeeId = orgAddMembers.EmployeeId
		//				});

		//			orgAddMembers.AccessCode = code.ToString();

		//			if (orgAddMembers.SubscriptionRoles != null)
		//			{
		//				foreach (SubscriptionRoleSelectionModel role in orgAddMembers.SubscriptionRoles)
		//				{
		//					if (!role.Disabled && role.SelectedRole != 0)
		//					{
		//						Service.CreateInvitationSubRole(invitationId, role.SubscriptionId, role.SelectedRole);
		//					}
		//				}
		//			}

		//			return orgAddMembers;
		//		}
		//	}

		//	return null;
		//}

		/// <summary>
		/// Removes the provided invitation from the invitations table.
		/// </summary>
		/// <param name="invitationId">The Invitation's Id.</param>
		/// <returns>Removing the User and redirecting to EditMembers/{id}.</returns>
		[HttpPost]
		public ActionResult RemoveInvitation(int invitationId)
		{
			if (Service.RemoveInvitation(invitationId))
			{
				Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.InvitationDeleteNotification, Variety.Success));

				return this.RedirectToAction(ActionConstants.Manage);
			}

			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Controllers.Auth.Strings.CannotEditMembersMessage), ControllerConstants.Account, ActionConstants.RemoveInvitation));
		}
	}
}
