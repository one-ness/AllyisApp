//------------------------------------------------------------------------------
// <copyright file="ManageAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Services;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;
using System;
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
		/// Get: Account/Manage/id
		/// The management page for an organization, displays billing, subscriptions, etc.
		/// </summary>
		/// <param name="id">The organization Id</param>
		/// <returns>The organization's management page.</returns>
		public ActionResult ManageOrg(int id)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
			ManageOrgViewModel model = this.ConstructOrganizationManageViewModel(id);
			return this.View(model);
		}

		/// <summary>
		/// Uses services to populate the lists of an <see cref="ManageOrgViewModel"/> and returns it.
		/// </summary>
		/// <returns>The OrganizationManageViewModel.</returns>
		[CLSCompliant(false)]
		public ManageOrgViewModel ConstructOrganizationManageViewModel(int orgId)
		{
			var infos = AppService.GetOrganizationManagementInfo(orgId);

			BillingServicesCustomer customer = (infos.Item5 == null) ? null : AppService.RetrieveCustomer(new BillingServicesCustomerId(infos.Item5));

			return new ManageOrgViewModel
			{
				Details = infos.Item1,
				LastFour = customer == null ? string.Empty : customer.Last4,
				Members = new OrganizationMembersViewModel
				{
					CurrentUserId = this.AppService.UserContext.UserId,
					DisplayUsers = infos.Item2.Select(oui => new OrganizationUserViewModel
					{
						Email = oui.Email,
						EmployeeId = oui.EmployeeId,
						FullName = string.Format("{0} {1}", oui.FirstName, oui.LastName),
						OrganizationId = oui.OrganizationId,
						PermissionLevel = ((OrganizationRole)oui.OrgRoleId).ToString(),
						UserId = oui.UserId,
						EmployeeTypeId = oui.EmployeeTypeId
					}),
					OrganizationId = infos.Item1.OrganizationId,
					OrganizationName = infos.Item1.Name,
					PendingInvitation = infos.Item4,
					TotalUsers = infos.Item2.Count
				},
				OrganizationId = orgId,
				BillingCustomer = customer,
				SubscriptionCount = infos.Item3.Count,
				Subscriptions = infos.Item6.Select(p =>
				{
					return new SubscriptionDisplayViewModel
					{
						Info = infos.Item3.Where(s => s.ProductId == p.ProductId).SingleOrDefault(),
						ProductId = p.ProductId,
						ProductName = p.ProductName,
						ProductDescription = p.ProductDescription,
						OrganizationId = orgId,
						AreaUrl = p.AreaUrl
					};
				})
			};
		}

		/// <summary>
		/// Edits the Employee Id on an OrgUser
		/// </summary>
		/// <param name="user">The user Id</param>
		/// <param name="org">The organization Id</param>
		/// <param name="employeeId">The new EmployeeId</param>
		/// <returns></returns>
		[HttpPost]
		public bool SaveEmployeeId(int user, int org, string employeeId)
		{
			try
			{
				var result = AppService.SetEmployeeId(user, org, employeeId);
				if (!result)
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Edits the Employee Id on an Invitation
		/// </summary>
		/// <param name="user">The Invitation Id</param>
		/// <param name="org">The Organization Id</param>
		/// <param name="employeeId">The new Employee Id</param>
		/// <returns>the result of this operation</returns>
		[HttpPost]
		public bool SaveInvitationEmployeeId(int user, int org, string employeeId)
		{
			try
			{
				var result = AppService.SetInvitationEmployeeId(user, org, employeeId);
				if (!result)
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Edits the Employee Type Id on an OrgUser
		/// </summary>
		/// <param name="user">The user Id</param>
		/// <param name="org">The organization Id</param>
		/// <param name="employeeTypeId">The new Employee Type Id</param>
		/// <returns></returns>
		[HttpPost]
		public bool SaveEmployeeTypeId(int user, int org, int employeeTypeId)
		{
			try
			{
				AppService.SetEmployeeTypeId(user, org, employeeTypeId);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Edits the Employee Type Id on an Invitation
		/// </summary>
		/// <param name="user">The Invitation Id</param>
		/// <param name="org">The organization Id</param>
		/// <param name="employeeTypeId">The new Employee Type Id</param>
		/// <returns></returns>
		[HttpPost]
		public bool SaveInvitationEmployeeTypeId(int user, int org, int employeeTypeId)
		{
			try
			{
				AppService.SetInvitationEmployeeTypeId(user, org, employeeTypeId);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
	}
}
