//------------------------------------------------------------------------------
// <copyright file="ManageAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
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
		/// Get: Account/Manage.
		/// The management page for an organization, displays billing, subscriptions, etc.
		/// </summary>
		/// <returns>The organization's management page.</returns>
		public ActionResult Manage()
		{
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				OrganizationManageViewModel model = this.ConstructOrganizationManageViewModel();
				return this.View(model);
			}

			Notifications.Add(new Core.Alert.BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Core.Alert.Variety.Warning));
			return this.RedirectToAction(ActionConstants.Index);
		}

		/// <summary>
		/// Uses services to populate the lists of an <see cref="OrganizationManageViewModel"/> and returns it.
		/// </summary>
		/// <returns>The OrganizationManageViewModel.</returns>
		[CLSCompliant(false)]
		public OrganizationManageViewModel ConstructOrganizationManageViewModel()
		{
			var infos = Service.GetOrganizationManagementInfo();

			BillingServicesCustomer customer = (infos.Item5 == null) ? null : Service.RetrieveCustomer(new BillingServicesCustomerId(infos.Item5));

			return new OrganizationManageViewModel
			{
				Details = infos.Item1,
				LastFour = customer == null ? string.Empty : customer.Last4,
				Members = new OrganizationMembersViewModel
				{
					CurrentUserId = UserContext.UserId,
					DisplayUsers = infos.Item2.Select(oui => new OrganizationUserViewModel
					{
						Email = oui.Email,
						EmployeeId = oui.EmployeeId,
						FullName = string.Format("{0} {1}", oui.FirstName, oui.LastName),
						OrganizationId = oui.OrganizationId,
						PermissionLevel = ((OrganizationRole)oui.OrgRoleId).ToString(),
						UserId = oui.UserId
					}),
					OrganizationId = infos.Item1.OrganizationId,
					OrganizationName = infos.Item1.Name,
					PendingInvitation = infos.Item4,
					TotalUsers = infos.Item2.Count
				},
				OrganizationId = UserContext.ChosenOrganizationId,
				BillingCustomer = customer,
				SubscriptionCount = infos.Item3.Count,
				Subscriptions = infos.Item6.Select(p =>
				{
					SubscriptionDisplayInfo sub = infos.Item3.Where(s => s.ProductId == p.ProductId).SingleOrDefault();
					if (sub == null)
					{
						return null;
					}
					else
					{
						return new SubscriptionDisplayViewModel
						{
							Info = sub,
							ProductId = p.ProductId,
							ProductName = p.ProductName,
							ProductDescription = p.ProductDescription
						};
					}
				})
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="user"></param>
		/// <param name="org"></param>
		/// <param name="employeeId"></param>
		/// <returns></returns>
		public bool SaveEmployeeId(int user, int org, string employeeId)
		{
			UserOrganizationInfo userOrgInfo = UserContext.UserOrganizationInfoList.Where(o => o.OrganizationId == org).SingleOrDefault();
			if (userOrgInfo == null)
			{
				return false;
			}

			Service.UpdateOrganizationUser(user, org, (int)userOrgInfo.OrganizationRole, employeeId);
			return true;
		}
	}
}
