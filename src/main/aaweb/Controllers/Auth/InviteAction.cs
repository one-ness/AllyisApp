//------------------------------------------------------------------------------
// <copyright file="InviteAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.Account;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.ViewModels;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// POST: Account/AddUsers/1.
		/// </summary>
		/// <param name="org">A Model containing the organization information and the string of emails to add.</param>
		/// <returns>Task actionResult.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Invite(OrganizationAddMembersViewModel org)
		{
			if (ModelState.IsValid)
			{
				if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditOrganization))
				{
					org.Organization = OrgService.GetOrganization(org.OrganizationId);
					org = await this.ProcessUserInput(org);
					
					foreach (string user in org.AddedUsers)
					{
						Notifications.Add(new BootstrapAlert(user + Resources.Controllers.Auth.Strings.UserAddedSuccessfully, Variety.Success));
					}

					foreach (string user in org.EmailedUsers)
					{
						Notifications.Add(new BootstrapAlert(user + Resources.Controllers.Auth.Strings.UserEmailed, Variety.Info));
					}

					foreach (string user in org.UsersAlreadyExisting)
					{
						Notifications.Add(new BootstrapAlert(user + Resources.Controllers.Auth.Strings.UserAlreadyExists, Variety.Warning));
					}
					
					return this.RedirectToAction("Manage");
				}

				// Permission failure
				return this.View("Error", new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Controllers.Auth.Strings.CannotEditMembersMessage), "Account", "Invite"));
			} 

			// Invalid model; try again
			return this.RedirectToAction("Manage");
		}

		//// TODO: If the services get consolidated into one class, this method could be moved to Services and a business object created to mirror OrganizationAddMembersViewModel.
		////        At the moment, the calls to different service objects would require one having its own reference to or instance of the others in order to move this.

		/// <summary>
		/// Processes an OrganizationAddMembersViewModel as input.
		/// </summary>
		/// <param name="orgAddMembers">The OrganizationAddMembersViewModel to process.</param>
		/// <returns>The OrganizationAddMembersViewModel after processing, with updated AddedUsers/UsersAlreadyExisting/EmailedUsers. Returns null if unsuccessful.</returns>
		public async Task<OrganizationAddMembersViewModel> ProcessUserInput(OrganizationAddMembersViewModel orgAddMembers)
		{
			if (!string.IsNullOrEmpty(orgAddMembers.Email))
			{
				string userEmail = orgAddMembers.Email.Trim();				
				if (AccountService.IsEmailAddressValid(userEmail))
				{ // If input string kinda looks like an email..
					UserInfo user = await AccountService.GetUserByEmail(userEmail); // ...Attempt to get the user data by email.
																						// If that doesn't return null...
					if (user != null)
					{
						OrgRoleInfo role = OrgService.GetOrgRole(orgAddMembers.OrganizationId, user.UserId); // ...see if they have permissions in this organization already
																																// If not...
					
						if (role == null)
						{
							////OrgService.AddToOrganization(user.UserId, orgAddMembers.OrganizationId, orgAddMembers.SubscriptionProjectId, orgAddMembers.AddAsOwner ? (int)OrganizationRole.Owner : (int)OrganizationRole.Member);
							////orgAddMembers.AddedUsers.Add(userEmail);
							////IEnumerable<SubscriptionDisplayInfo> subs = CrmService.GetSubscriptionsDisplayByOrg(orgAddMembers.OrganizationId);

							////foreach (SubscriptionRoleSelectionModel subRole in orgAddMembers.SubscriptionRoles)
							////{
							////	SubscriptionDisplayInfo currentSub = subs.Where(x => x.SubscriptionId == subRole.SubscriptionId).SingleOrDefault();
							////	if (currentSub != null && currentSub.SubscriptionsUsed < currentSub.NumberOfUsers)
							////	{
							////		OrgService.UpdateSubscriptionUserProductRole(subRole.SelectedRole, subRole.SubscriptionId, user.UserId);
							////	}
							////}
						}
						else
						{
							orgAddMembers.UsersAlreadyExisting.Add(userEmail);
							return orgAddMembers;
						}
					}
					////else
					////{
						// input string is not associated with an existing user
						// so send them an email and let them know of the request
						orgAddMembers.EmailedUsers.Add(userEmail);
						UserInfo requestingUser = AccountService.GetUserInfo();
						int code = new Random().Next(100000);
						int invitationId = await OrgService.InviteNewUser(
							string.Format("{0} {1}", requestingUser.FirstName, requestingUser.LastName),
							GlobalSettings.WebRoot,
							new InvitationInfo
							{
								Email          = userEmail,
								FirstName      = orgAddMembers.FirstName,
								LastName       = orgAddMembers.LastName,
								OrganizationId = orgAddMembers.OrganizationId,
								AccessCode     = code.ToString(),
								DateOfBirth    = DateTime.MinValue.AddYears(1754),
								OrgRole        = (int)(orgAddMembers.AddAsOwner ? OrganizationRole.Owner : OrganizationRole.Member),
								ProjectId      = orgAddMembers.SubscriptionProjectId
							});

						orgAddMembers.AccessCode = code.ToString();

						if (orgAddMembers.SubscriptionRoles != null)
						{
							foreach (SubscriptionRoleSelectionModel role in orgAddMembers.SubscriptionRoles)
							{
								if (!role.Disabled && role.SelectedRole != 0)
								{
									OrgService.CreateInvitationSubRole(invitationId, role.SubscriptionId, role.SelectedRole);
								}
							}
						}
					////}

					return orgAddMembers;
				}
			}

			return null;
		}

		/// <summary>
		/// Removes the provided invitation from the invitations table.
		/// </summary>
		/// <param name="invitationId">The Invitation's Id.</param>
		/// <returns>Removing the User and redirecting to EditMembers/{id}.</returns>
		[HttpPost]
		public ActionResult RemoveInvitation(int invitationId)
		{
			if (OrgService.RemoveInvitation(invitationId))
			{
				Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.InvitationDeleteNotification, Variety.Success));

				return this.RedirectToAction("Manage");
			}

			return this.View("Error", new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Controllers.Auth.Strings.CannotEditMembersMessage), "Account", "RemoveInvitation"));
		}
	}
}
