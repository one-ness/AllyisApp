//------------------------------------------------------------------------------
// <copyright file="AddAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="AddAsOwner">Boolean passed from Invite Action</param>
        /// <param name="Email">Email Passed from Invite Action</param>
        /// <param name="FirstName">First Name passed from Invite Action</param>
        /// <param name="LastName">Last Name passed from Invite Action</param>
        /// <param name="SelectedRole">Selected Role passed from Invtite Action</param>
        /// <param name="SelectedProject">Selected Project passed from Invite Action</param>
        /// <returns>The result of this action.</returns>
        public ActionResult Add(string returnUrl, string FirstName, string LastName, string Email, bool AddAsOwner = false, int SelectedRole = 0, int SelectedProject = 0)
		{
			// Only owners should view this page
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				AddMemberViewModel model = ConstructOrganizationAddMembersViewModel();
                if (FirstName != null)
                {
                    model.FirstName = FirstName;
                    model.LastName = LastName;
                    model.Email = Email;
                    model.AddAsOwner = AddAsOwner;
                    model.Subscriptions.First().SelectedRole = SelectedRole;
                    model.SubscriptionProjectId = SelectedProject;
                }
                ViewBag.returnUrl = returnUrl;
				return this.View(model);
			}

			ViewBag.ErrorInfo = "Permission";
			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Errors.CannotEditMembersMessage), ControllerConstants.Account, ActionConstants.Add));
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
