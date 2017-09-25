//------------------------------------------------------------------------------
// <copyright file="SettingsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Crm;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Staffing controller.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Index
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public ActionResult Settings(int subscriptionId)
		{
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			string Subname = AppService.getSubscriptionName(subscriptionId);
			var infos = AppService.GetStaffingIndexInfo(subInfo.OrganizationId);

			//ViewBag.SignedInUserID = GetCookieData().UserId;
			//ViewBag.SelectedUserId = userId;

			StaffingSettingsViewModel model = this.ConstructStaffingSettingsViewModel(
				subInfo.OrganizationId,
				subscriptionId,
				Subname,
				infos.Item2, //tags list
				infos.Item3, //employmentTypes list
				infos.Item4, //positionLevels list
				infos.Item5,  //positionStatuses list
				infos.Item6
				);

			return this.View(model);
		}

		/// <summary>
		/// construct the staffing page
		/// </summary>
		/// <param name="orgId"></param>
		/// <param name="subId"></param>
		/// <param name="subName"></param>
		/// <param name="tags"></param>
		/// <param name="employmentTypes"></param>
		/// <param name="positionLevelsList"></param>
		/// <param name="positionStatuses"></param>
		/// <param name="customers"></param>
		/// <returns></returns>
		public StaffingSettingsViewModel ConstructStaffingSettingsViewModel(int orgId, int subId, string subName,
						List<Services.Lookup.Tag> tags, List<EmploymentType> employmentTypes, List<PositionLevel> positionLevelsList,
						List<PositionStatus> positionStatuses, List<Customer> customers)
		{
			StaffingSettingsViewModel result = new StaffingSettingsViewModel()
			{
				organizationId = orgId,
				subscriptionId = subId,
				subscriptionName = subName,
				tags = tags,
				employmentTypes = employmentTypes,
				positionLevels = positionLevelsList,
				positionStatuses = positionStatuses,
				customers = customers
			};

			return result;
		}
	}
}