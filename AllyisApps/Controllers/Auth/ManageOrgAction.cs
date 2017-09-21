//------------------------------------------------------------------------------
// <copyright file="ManageOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Lib;
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
				ViewBag.StartDate = Utility.GetDaysFromDateTime(SetStartingDate(startOfWeek));
				ViewBag.EndDate = Utility.GetDaysFromDateTime(SetStartingDate(startOfWeek).AddDays(6));
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
			var orgInfo = AppService.GetOrganizationManagementInfo(organizationId);

			BillingServicesCustomer customer = (orgInfo.StripeToken == null) ? null : AppService.RetrieveCustomer(new BillingServicesCustomerId(orgInfo.StripeToken));
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
					DisplayUsers = orgInfo.Users.Select(oui => new OrganizationUserViewModel
					{
						Email = oui.Email,
						EmployeeId = oui.EmployeeId,
						FullName = string.Format("{0} {1}", oui.FirstName, oui.LastName),
						OrganizationId = oui.OrganizationId,
						PermissionLevel = ((OrganizationRole)oui.OrganizationRoleId).ToString(),
						UserId = oui.UserId
					}),
					OrganizationId = orgInfo.OrganizationId,
					OrganizationName = orgInfo.OrganizationName,
					PendingInvitation = orgInfo.Invitations.Select(invite => new InvitationInfoViewModel(invite)),
					TotalUsers = orgInfo.Users.Count
				},
				OrganizationId = organizationId,
				BillingCustomer = customer,
				SubscriptionCount = orgInfo.Subscriptions.Count,
				Subscriptions = orgInfo.Subscriptions.Select(sub => new SubscriptionDisplayViewModel(sub))
			};
		}
	}
}