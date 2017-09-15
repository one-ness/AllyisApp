//------------------------------------------------------------------------------
// <copyright file="ManageOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;

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
		/// <param name="id">The organization Id.</param>
		/// <returns>The organization's management page.</returns>
		public ActionResult ManageOrg(int id)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
			ManageOrgViewModel model = this.ConstructOrganizationManageViewModel(id);

			var sub = model.Subscriptions.Select(x => x).Where(y => y.ProductId == (int)ProductIdEnum.TimeTracker).FirstOrDefault();
			if (sub != null && model.Subscriptions.Count() > 0)
			{
				int subId = sub.SubscriptionId;
				int startOfWeek = AppService.GetAllSettings(subId).Item1.StartOfWeek;
				ViewBag.StartDate = AppService.GetDaysFromDateTime(SetStartingDate(startOfWeek));
				ViewBag.EndDate = AppService.GetDaysFromDateTime(SetStartingDate(startOfWeek).AddDays(6));
			}

			ViewData["UserId"] = this.AppService.UserContext.UserId;
			return this.View(model);
		}

		/// <summary>
		/// Uses services to populate the lists of an <see cref="ManageOrgViewModel"/> and returns it.
		/// </summary>
		/// <param name="organizationId">Organization id.</param>
		/// <returns>The OrganizationManageViewModel.</returns>
		[CLSCompliant(false)]
		public ManageOrgViewModel ConstructOrganizationManageViewModel(int organizationId)
		{
			var infos = AppService.GetOrganizationManagementInfo(organizationId);

			BillingServicesCustomer customer = (infos.Item5 == null) ? null : AppService.RetrieveCustomer(new BillingServicesCustomerId(infos.Item5));
			Organization orgInfo = infos.Item1;
			return new ManageOrgViewModel
			{
				Details = new OrganizationInfoViewModel()
				{
					OrganizationName = orgInfo.OrganizationName,
					OrganizaitonId = orgInfo.OrganizationId,
					SiteURL = orgInfo.SiteUrl,
					FaxNumber = orgInfo.FaxNumber,
					PhoneNumber = orgInfo.PhoneNumber,
					Address = orgInfo.Address?.Address1,
					City = orgInfo.Address?.City,
					CountryName = orgInfo.Address?.CountryName,
					StateName = orgInfo.Address?.StateName,
					PostalCode = orgInfo.Address?.PostalCode
				},
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
						PermissionLevel = ((OrganizationRole)oui.OrganizationRoleId).ToString(),
						UserId = oui.UserId
					}),
					OrganizationId = infos.Item1.OrganizationId,
					OrganizationName = infos.Item1.OrganizationName,
					PendingInvitation = infos.Item4.Select(invite => new InvitationInfoViewModel(invite)),
					TotalUsers = infos.Item2.Count
				},
				OrganizationId = organizationId,
				BillingCustomer = customer,
				SubscriptionCount = infos.Item3.Count,
				Subscriptions = infos.Item3.Select(sub => new SubscriptionDisplayViewModel(sub))
			};
		}
	}
}