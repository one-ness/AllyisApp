﻿//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Services.StaffingManager;
using System.Collections.Generic;

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
		public ActionResult Index(int subscriptionId)
		{

			UserSubscription subInfo = null;
			this.AppService.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);

			var infos = AppService.GetStaffingIndexInfo(subInfo.OrganizationId);
			
			//ViewBag.SignedInUserID = GetCookieData().UserId;
			//ViewBag.SelectedUserId = userId;
			
			StaffingIndexViewModel model = this.ConstructStaffingIndexViewModel(
				subInfo.OrganizationId,
				subscriptionId,
				subInfo.SubscriptionName,
				infos.Item1, //positions list
				infos.Item2, //tags list
				infos.Item3, //employmentTypes list
				infos.Item4, //positionLevels list
				infos.Item5  //positionStatuses list
				);
			return this.View(model);
		}

		/// <summary>
		/// Index 
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <param name="statusFilterName"></param>
		/// <param name="typeFilterName"></param>
		/// <param name="tagsFilter"></param>
		/// <returns></returns>
		[Route("{StaffingManager}/{Index}/{Status?}/{Type?}/{Tags?}")]
		public ActionResult IndexFiltered(int subscriptionId, string statusFilterName, string typeFilterName, List<string> tagsFilter)
		{

			UserSubscription subInfo = null;
			this.AppService.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);

			var infos = AppService.GetStaffingIndexInfoFiltered(subInfo.OrganizationId, statusFilterName, typeFilterName, tagsFilter);

			//ViewBag.SignedInUserID = GetCookieData().UserId;
			//ViewBag.SelectedUserId = userId;

			StaffingIndexViewModel model = this.ConstructStaffingIndexViewModel(
				subInfo.OrganizationId,
				subscriptionId,
				subInfo.SubscriptionName,
				infos.Item1, //positions list
				infos.Item2, //tags list
				infos.Item3, //employmentTypes list
				infos.Item4, //positionLevels list
				infos.Item5  //positionStatuses list
				);
			return this.View(model);
		}


		/// <summary>
		/// Constructor for the TimeEntryOverDateRangeViewModel.
		/// </summary>
		/// <param name="orgId">The Organization Id.</param>
		/// <param name="subId">The Subscription's Id.</param>
		/// <param name="subName">The Subscription's Name.</param>
		/// <param name="positions">The positions list to be displayed.</param>
		/// <param name="tags">tags used by the org.</param>
		/// <param name="employmentTypes">The employment types used by the org.</param>
		/// <param name="positionLevels">The position levels used by the orgs.</param>
		/// <param name="positionStatuses">The position statuses used by the org.</param>
		/// <returns>The constructed TimeEntryOverDateRangeViewModel.</returns>
		public StaffingIndexViewModel ConstructStaffingIndexViewModel(int orgId, int subId, string subName, 
						List<PositionThumbnailInfo> positions, List<Services.Lookup.Tag> tags, List<EmploymentType> employmentTypes, 
						List<PositionLevel> positionLevels, List<PositionStatus> positionStatuses)
		{
			StaffingIndexViewModel result = new StaffingIndexViewModel()
			{
				organizationId = orgId,
				subscriptionId = subId,
				subscriptionName = subName,
				positions = positions,
				tags = tags,
				employmentTypes = employmentTypes,
				positionLevels = positionLevels,
				positionStatuses = positionStatuses
			};

			return result;
		}
	}
}