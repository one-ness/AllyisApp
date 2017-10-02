//------------------------------------------------------------------------------
// <copyright file="ViewPositionAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Dynamic;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.StaffingManager;
using AllyisApps.ViewModels;
using System.Web.Script.Serialization;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Position.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// GET: Position/Create.
		/// </summary>
		/// <param name="positionId">The position id.</param>
		/// <param name="subscriptionId">the subscription</param>
		/// <returns>Presents a page for the creation of a new position.</returns>
		public ActionResult ViewPosition(int positionId, int subscriptionId)
		{
			var viewModel = setupViewPositionViewModel(positionId, subscriptionId);

			return this.View(viewModel);
		}

		/// <summary>
		/// setup position setup viewmodel
		/// </summary>
		/// <returns></returns>
		public ViewPositionViewModel setupViewPositionViewModel(int positionId, int subscriptionId)
		{
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			Position pos = AppService.GetPosition(positionId);
			List<Application> applications = AppService.GetFullApplicationInfoByPositionId(positionId);

			string subscriptionNameToDisplay = AppService.getSubscriptionName(subscriptionId);
			//TODO: this is piggy-backing off the get index action, create a new action that just gets items 3-5.
			var infos = AppService.GetStaffingIndexInfo(subInfo.OrganizationId);
			var temp = new string[infos.Item2.Count];
			var count = 0;
			var assignedTags = "";
			for (int i = 0; i < infos.Item2.Count; i++)
			{
				bool taken = false;
				for (int j = 0; j < i; j++)
				{
					if (infos.Item2[i].TagName == temp[j] && !taken) taken = true;
				}
				if (!taken)
				{
					temp[count] = infos.Item2[i].TagName;
					count++;
				}
			}
			var tags = new string[count];
			for (int k = 0; k < count; k++) tags[k] = temp[k];
			foreach(var tag in pos.Tags) assignedTags += "," + tag.TagName;
			return new ViewPositionViewModel
			{
				PositionId = pos.PositionId,
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService),
				LocalizedStates = new Dictionary<string, string>(),
				IsCreating = false,
				OrganizationId = subInfo.OrganizationId,
				SubscriptionName = subscriptionNameToDisplay,
				SubscriptionId = subInfo.SubscriptionId,
				StartDate = pos.StartDate,
				Tags = tags,
				EmploymentTypes = infos.Item3.AsParallel().Select(et => new EmploymentTypeSelectViewModel()
				{
					EmploymentTypeId = et.EmploymentTypeId,
					EmploymentTypeName = et.EmploymentTypeName
				}).ToList(),
				PositionLevels = infos.Item4.AsParallel().Select(pl => new PositionLevelSelectViewModel()
				{
					PositionLevelId = pl.PositionLevelId,
					PositionLevelName = pl.PositionLevelName
				}).ToList(),
				PositionStatuses = infos.Item5.AsParallel().Select(ps => new PositionStatusSelectViewModel()
				{
					PositionStatusId = ps.PositionStatusId,
					PositionStatusName = ps.PositionStatusName
				}).ToList(),
				Customers = infos.Item6.AsParallel().Select(cus => new CustomerSelectViewModel()
				{
					CustomerId = cus.CustomerId,
					CustomerName = cus.CustomerName
				}).ToList(),
				Applications = applications,
				CustomerId = pos.CustomerId,
				AddressId = pos.AddressId,
				PositionTitle = pos.PositionTitle,
				BillingRateAmount = pos.BillingRateAmount,
				BillingRateFrequency = (BillingRateEnum)pos.BillingRateFrequency,
				DurationMonths = pos.DurationMonths,
				EmploymentTypeId = pos.EmploymentTypeId,
				PositionStatusId = pos.PositionStatusId,
				PositionCount = pos.PositionCount,
				RequiredSkills = pos.RequiredSkills,
				JobResponsibilities = pos.JobResponsibilities,
				DesiredSkills = pos.DesiredSkills,
				PositionLevelId = pos.PositionLevelId,
				HiringManager = pos.HiringManager,
				TeamName = pos.TeamName,
				TagsToSubmit = assignedTags,
				PositionAddress = new AddressViewModel {
					Country = pos.Address.CountryName,
					City = pos.Address.City,
					State = pos.Address.StateName
				}
			};
		}
	}
}