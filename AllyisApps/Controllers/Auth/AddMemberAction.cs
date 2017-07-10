//------------------------------------------------------------------------------
// <copyright file="AddAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

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
		/// <param name="id"></param>
		/// <param name="returnUrl">The return url to redirect to after form submit.</param>
		/// <returns>The result of this action.</returns>
		public ActionResult AddMember(int id, string returnUrl)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
			AddMemberViewModel model = ConstructOrganizationAddMembersViewModel(id);
			ViewBag.returnUrl = returnUrl;
			return this.View(model);
		}

		/// <summary>
		/// POST: /Add
		/// Adding a new member to an organization.
		/// </summary>
		/// <param name="add">The View Model of user info passed from Add.cshtml</param>
		/// <param name="id"></param>
		/// <returns>The result of this action</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> AddMember(AddMemberViewModel add, int id)
		{
			AddMemberViewModel model = ConstructOrganizationAddMembersViewModel(id);
			add.Subscriptions = model.Subscriptions;
			add.Projects = model.Projects;

			if (ModelState.IsValid)
			{
				this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, add.OrganizationId);
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
					int invitationId = await AppService.InviteUser(
						Url.Action(ActionConstants.Register, ControllerConstants.Account, new { accessCode = "{accessCode}" }, protocol: Request.Url.Scheme),
						new InvitationInfo
						{
							Email = add.Email.Trim(),
							FirstName = add.FirstName,
							LastName = add.LastName,
							OrganizationId = add.OrganizationId,
							OrgRole = (int)(add.AddAsOwner ? OrganizationRole.Owner : OrganizationRole.Member),
							ProjectId = add.SubscriptionProjectId,
							EmployeeId = add.EmployeeId,
							EmployeeType = (int)(add.EmployeeType == "Salaried" ? EmployeeType.Salaried : EmployeeType.Hourly)
						},
						subId,
						subRoleId
					);

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
					Notifications.Add(new BootstrapAlert(string.Format("{0} {1} " + Resources.Strings.UserAlreadyExists, add.FirstName, add.LastName), Variety.Warning));
					return this.View(add);
				}
				catch (System.Data.DuplicateNameException)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.EmployeeIdNotUniqueError, Variety.Danger));
					return this.View(add);
				}
			}

			// Invalid model; try again
			return this.View(add);
		}

		/// <summary>
		/// Uses services to populate the lists of an <see cref="AddMemberViewModel"/> and returns it.
		/// </summary>
        /// <param name="id">The Organization Id</param>
		/// <returns>The OrganizationAddMembersViewModel.</returns>
		public AddMemberViewModel ConstructOrganizationAddMembersViewModel(int id)
		{
			var infos = AppService.GetAddMemberInfo(id);
			string nextId = string.Compare(infos.Item1, infos.Item5) > 0 ? infos.Item1 : infos.Item5;

			AddMemberViewModel result = new AddMemberViewModel
			{
				OrganizationId = id,
				EmployeeId = new string(AppService.IncrementAlphanumericCharArray(nextId.ToCharArray())),
				Subscriptions = new List<AddMemberSubscriptionInfo>(),
				Projects = infos.Item4,
			};

			foreach (SubscriptionDisplayInfo sub in infos.Item2)
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
					ProductRoleId = (int)TimeTrackerRole.User
				});
				result.Subscriptions.Add(subInfo);
			}

			return result;
		}
	}
}
