//------------------------------------------------------------------------------
// <copyright file="AddAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
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
        /// GET: /Add.
        /// The page for adding members to an organization.
        /// </summary>
        /// <param name="returnUrl">The return url to redirect to after form submit.</param>
        /// <returns>The result of this action.</returns>
        public ActionResult Add(string returnUrl)
		{
			// Only owners should view this page
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				AddMemberViewModel model = ConstructOrganizationAddMembersViewModel();
                ViewBag.returnUrl = returnUrl;
				return this.View(model);
			}

			ViewBag.ErrorInfo = "Permission";
			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.CannotEditMembersMessage), ControllerConstants.Account, ActionConstants.Add));
		}

        /// <summary>
        /// POST: /Add
        /// Adding a new member to an organization.
        /// </summary>
        /// <param name="add">The View Model of user info passed from Add.cshtml</param>
        /// <returns>The result of this action</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddMemberViewModel add)
        {
            AddMemberViewModel model = ConstructOrganizationAddMembersViewModel();
            add.Subscriptions = model.Subscriptions;
            add.Projects = model.Projects;

            if (ModelState.IsValid)
            {
                if (Service.Can(Actions.CoreAction.EditOrganization))
                {
                    int? subId = null, subRoleId = null;
                    if (add.Subscriptions != null && add.Subscriptions.Count > 0)
                    {
                        var sub = add.Subscriptions.First();
                        if (sub.SelectedRole != 0)
                        {
                            subId = sub.SubscriptionId;
                            subRoleId = sub.SelectedRole;
                        }
                    }

                    try
                    {
                        int invitationId = await Service.InviteUser(
                            Url.Action(ActionConstants.Index, ControllerConstants.Account, new { accessCode = "{accessCode}" }, protocol: Request.Url.Scheme),
                            new InvitationInfo
                            {
                                Email = add.Email.Trim(),
                                FirstName = add.FirstName,
                                LastName = add.LastName,
                                OrganizationId = add.OrganizationId,
                                OrgRole = (int)(add.AddAsOwner ? OrganizationRole.Owner : OrganizationRole.Member),
                                ProjectId = add.SubscriptionProjectId,
                                EmployeeId = add.EmployeeId
                            },
                            subId,
                            subRoleId
                        );

                        Notifications.Add(new BootstrapAlert("User has been invited to join the organization.", Variety.Success));
                        return this.RedirectToAction(ActionConstants.Manage);

                    }
                    catch (ArgumentException ex)
                    {
                        
                        if (ex.ParamName.Equals("invitationInfo.Email"))
                        {
                            Notifications.Add(new BootstrapAlert("Email address is invalid.", Variety.Danger));
                            return this.View(add);
                        }

                        throw ex;

                    }
                    catch (InvalidOperationException)
                    {
                        Notifications.Add(new BootstrapAlert("This user is already a member of the organization.", Variety.Warning));
                        return this.View(add);
                    }
                    catch (System.Data.DuplicateNameException)
                    {
                        Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.EmployeeIdNotUniqueError, Variety.Danger));
                        return this.View(add);
                    }


                    //// Employee Id must be unique; check in a union of invites and current org members
                    //// TODO: Make a db procedure and all subsequent methods to simply grab all of the ids instead of using this list union
                    //if (Service.GetOrganizationMemberList(this.UserContext.ChosenOrganizationId).Select(user => user.EmployeeId).ToList().Union(
                    //	Service.GetUserInvitations().Select(invitation => invitation.EmployeeId).ToList()).Any(id => id == add.EmployeeId))
                    //{
                    //	Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.EmployeeIdNotUniqueError, Variety.Danger));
                    //	return this.RedirectToAction(ActionConstants.Add);
                    //}

                    //if (!string.IsNullOrEmpty(add.Email))
                    //{
                    //	string userEmail = add.Email.Trim();
                    //	if (Service.IsEmailAddressValid(userEmail))
                    //	{ // If input string kinda looks like an email..
                    //		User user = Service.GetUserByEmail(userEmail); // ...Attempt to get the user data by email.
                    //														   // If that doesn't return null...
                    //		if (user != null)
                    //		{
                    //			OrgRole role = Service.GetOrgRole(add.OrganizationId, user.UserId); // ...see if they have permissions in this organization already
                    //																							  // If not...
                    //			if (role != null)
                    //			{
                    //				Notifications.Add(new BootstrapAlert("This user is already a member of the organization.", Variety.Warning));
                    //				return this.RedirectToAction(ActionConstants.Add);
                    //			}
                    //		}

                    //		// input string is not associated with an existing user
                    //		// so send them an email and let them know of the request
                    //		User requestingUser = Service.GetUser();
                    //		int code = new Random().Next(100000);
                    //		int invitationId = await Service.InviteNewUser(
                    //			string.Format("{0} {1}", requestingUser.FirstName, requestingUser.LastName),
                    //			GlobalSettings.WebRoot,
                    //			new InvitationInfo
                    //			{
                    //				Email = userEmail,
                    //				FirstName = add.FirstName,
                    //				LastName = add.LastName,
                    //				OrganizationId = add.OrganizationId,
                    //				AccessCode = code.ToString(),
                    //				DateOfBirth = DateTime.MinValue.AddYears(1754),
                    //				OrgRole = (int)(add.AddAsOwner ? OrganizationRole.Owner : OrganizationRole.Member),
                    //				ProjectId = add.SubscriptionProjectId,
                    //				EmployeeId = add.EmployeeId
                    //			});

                    //		if (add.Subscriptions != null)
                    //		{
                    //			foreach (AddMemberSubscriptionInfo sub in add.Subscriptions)
                    //			{
                    //				if (!sub.hasTooManySubscribers && sub.SelectedRole != 0)
                    //				{
                    //					Service.CreateInvitationSubRole(invitationId, sub.SubscriptionId, sub.SelectedRole);
                    //				}
                    //			}
                    //		}

                    //		Notifications.Add(new BootstrapAlert("User has been invited to join the organization.", Variety.Success));
                    //		return this.RedirectToAction(ActionConstants.Manage);
                    //	}
                    //}

                    //Notifications.Add(new BootstrapAlert("Email address in invalid.", Variety.Danger));
                    //return this.RedirectToAction(ActionConstants.Add);
                }

                // Permission failure
                return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Controllers.Auth.Strings.CannotEditMembersMessage), ControllerConstants.Account, ActionConstants.Add));
            }

            // Invalid model; try again
            return this.View(add);
        }

        /// <summary>
        /// Uses services to populate the lists of an <see cref="AddMemberViewModel"/> and returns it.
        /// </summary>
        /// <returns>The OrganizationAddMembersViewModel.</returns>
        public AddMemberViewModel ConstructOrganizationAddMembersViewModel()
		{
			var infos = Service.GetAddMemberInfo();
			string nextId = string.Compare(infos.Item1, infos.Item5) > 0 ? infos.Item1 : infos.Item5;

			AddMemberViewModel result = new AddMemberViewModel
			{
				OrganizationId = UserContext.ChosenOrganizationId,
				EmployeeId = new string(Service.IncrementAlphanumericCharArray(nextId.ToCharArray())),
				Subscriptions = new List<AddMemberSubscriptionInfo>(),
				Projects = infos.Item4,
			};
            
			foreach(SubscriptionDisplayInfo sub in infos.Item2)
			{
				AddMemberSubscriptionInfo subInfo = new AddMemberSubscriptionInfo
				{
					ProductName = sub.ProductName,
					ProductRoles = infos.Item3.Where(r => r.ProductId == sub.ProductId).ToList(),
					SubscriptionId = sub.SubscriptionId,
					hasTooManySubscribers = sub.SubscriptionsUsed >= sub.NumberOfUsers
				};
				subInfo.ProductRoles.Insert(0, new ProductRole
				{
					Name = "None",
					ProductId = (int)ProductIdEnum.None,
					ProductRoleId = (int)ProductRoleIdEnum.NotInProduct
				});
				result.Subscriptions.Add(subInfo);
			}

			return result;
		}
	}
}
