﻿//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.StaffingManager;
using AllyisApps.Services.Lookup;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Staffing controller.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Index page.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="Statuses"></param>
		/// <param name="Types"></param>
		/// <param name="Tags"></param>
		/// <returns>The index view.</returns>
		public ActionResult Index(int subscriptionId, IList<string> Statuses, IList<string> Types, IList<string> Tags)
		{
			UserContext.SubscriptionAndRole subInfo = null;
			int userId = this.AppService.UserContext.UserId;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			string subName = AppService.getSubscriptionName(subscriptionId);

			System.Tuple<List<PositionThumbnailInfo>, List<Tag>, List<EmploymentType>, List<PositionLevel>, List<PositionStatus>, List<Customer>> infos;

			if ((Statuses != null  && Statuses.Count != 0) || (Types.Count != 0 && Types != null) || (Tags.Count == 0 && Tags != null))
			{
				List<string> statuses = new List<string>();
				if(Statuses != null) statuses = new List<string>(Statuses);
				List<string> types = new List<string>();
				if (Types != null) types = new List<string>(Types);
				List<string> tags = new List<string>();
				if (Tags != null) tags = new List<string>(Tags);

				infos = AppService.GetStaffingIndexInfoFiltered(subInfo.OrganizationId, statuses, types, tags, userId);
			}
			else
			{
				infos = AppService.GetStaffingIndexInfo(subInfo.OrganizationId, userId);
			}

			// ViewBag.SignedInUserID = GetCookieData().UserId;
			// ViewBag.SelectedUserId = userId;

			StaffingIndexViewModel model = this.ConstructStaffingIndexViewModel(
				subInfo.OrganizationId,
				subscriptionId,
				subName,
				infos.Item1, //positions list
				infos.Item2, //tags list
				infos.Item3, //employmentTypes list
				infos.Item4, //positionLevels list
				infos.Item5  //positionStatuses list
				);

			foreach (PositionThumbnailInfo pos in model.Positions)
			{
				foreach (Customer cus in infos.Item6)
				{
					if (pos.CustomerId == cus.CustomerId) pos.CustomerName = cus.CustomerName;
				}
			}

			return this.View(model);
		}

		/// <summary>
		/// Constructor for the TimeEntryOverDateRangeViewModel.
		/// </summary>
		/// <param name="orgId">The Organization Id.</param>
		/// <param name="subId">The Subscription's Id.</param>
		/// <param name="subName">The Subscription's Name.</param>
		/// <param name="positions">The positions list to be displayed.</param>
		/// <param name="tags">Tags used by the org.</param>
		/// <param name="employmentTypes">The employment types used by the org.</param>
		/// <param name="positionLevels">The position levels used by the orgs.</param>
		/// <param name="positionStatuses">The position statuses used by the org.</param>
		/// <returns>The constructed TimeEntryOverDateRangeViewModel.</returns>
		public StaffingIndexViewModel ConstructStaffingIndexViewModel(
			int orgId,
			int subId,
			string subName,
			List<PositionThumbnailInfo> positions,
			List<Services.Lookup.Tag> tags,
			List<EmploymentType> employmentTypes,
			List<PositionLevel> positionLevels,
			List<PositionStatus> positionStatuses)
		{
			StaffingIndexViewModel result = new StaffingIndexViewModel()
			{
				OrganizationId = orgId,
				SubscriptionId = subId,
				SubscriptionName = subName,
				Positions = positions,
				Tags = tags,
				EmploymentTypes = employmentTypes.AsParallel().Select(et => new EmploymentTypeViewModel()
				{
					EmploymentTypeId = et.EmploymentTypeId,
					EmploymentTypeName = et.EmploymentTypeName
				}).ToList(),
				PositionLevels = positionLevels.AsParallel().Select(pl => new PositionLevelViewModel()
				{
					PositionLevelId = pl.PositionLevelId,
					PositionLevelName = pl.PositionLevelName
				}).ToList(),
				PositionStatuses = positionStatuses.AsParallel().Select(ps => new PositionStatusViewModel()
				{
					PositionStatusId = ps.PositionStatusId,
					PositionStatusName = ps.PositionStatusName
				}).ToList(),
			};

			return result;
		}
	}
}