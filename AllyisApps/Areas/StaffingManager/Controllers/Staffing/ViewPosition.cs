//------------------------------------------------------------------------------
// <copyright file="ViewPositionAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.StaffingManager;
using AllyisApps.ViewModels;

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
		public async Task<ActionResult> ViewPosition(int positionId, int subscriptionId)
		{
			SetNavData(subscriptionId);

			var viewModel = await setupViewPositionViewModel(positionId, subscriptionId);

			return View(viewModel);
		}

		/// <summary>
		/// setup position setup viewmodel
		/// </summary>
		/// <returns></returns>
		public async Task<ViewPositionViewModel> setupViewPositionViewModel(int positionId, int subscriptionId)
		{
			UserContext.SubscriptionAndRole subInfo = null;
			AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			Position pos = await AppService.GetPosition(positionId);
			List<Application> applicationsSerive = await AppService.GetFullApplicationInfoByPositionId(positionId);
			List<ApplicationInfoViewModel> applications = new List<ApplicationInfoViewModel>();

			var subscriptionNameToDisplayTask = AppService.GetSubscriptionName(subscriptionId);
			//TODO: this is piggy-backing off the get index action, create a new action that just gets items 3-5.
			var infosTask = AppService.GetStaffingIndexInfo(subInfo.OrganizationId);

			await Task.WhenAll(new Task[] { infosTask, subscriptionNameToDisplayTask });

			var infos = infosTask.Result;
			var subscriptionNameToDisplay = subscriptionNameToDisplayTask.Result;

			foreach (Application app in applicationsSerive)
			{
				var viewApp = BuildApplications(app);
				viewApp.ApplicationStatuses = infos.Item6.AsParallel().Select(appStat => new ApplicationStatusSelectViewModel()
				{
					ApplicationStatusId = appStat.ApplicationStatusId,
					ApplicationStatusName = appStat.ApplicationStatusName
				}).ToList();
				applications.Add(viewApp);
			}
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
				if (!taken && infos.Item2[i].PositionId == pos.PositionId)
				{
					temp[count] = infos.Item2[i].TagName;
					count++;
				}
			}
			var tags = new string[count];
			for (int k = 0; k < count; k++) tags[k] = temp[k];
			DateTime formatingStartDate = new DateTime(); //this formating variable is nessicary if the view doesnt want to include time of day. You can't ToShortDateFormat a nullable DateTime
			formatingStartDate = DateTime.Now;
			if (pos.StartDate != null) formatingStartDate = (DateTime)pos.StartDate;
			foreach (var tag in pos.Tags) assignedTags += "," + tag.TagName;
			return new ViewPositionViewModel
			{
				PositionId = pos.PositionId,
				LocalizedCountries = ModelHelper.GetLocalizedCountries(AppService),
				LocalizedStates = new Dictionary<string, string>(),
				IsCreating = false,
				OrganizationId = subInfo.OrganizationId,
				SubscriptionName = subscriptionNameToDisplay,
				SubscriptionId = subInfo.SubscriptionId,
				StartDate = pos.StartDate,
				StartDateFormat = formatingStartDate,
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
				ApplicationStatuses = infos.Item6.AsParallel().Select(appStat => new ApplicationStatusSelectViewModel()
				{
					ApplicationStatusId = appStat.ApplicationStatusId,
					ApplicationStatusName = appStat.ApplicationStatusName
				}).ToList(),
				Customers = infos.Item7.AsParallel().Select(cus => new CustomerSelectViewModel()
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
				PositionAddress = new AddressViewModel
				{
					Country = pos.Address.CountryName,
					City = pos.Address.City,
					State = pos.Address.StateName
				}
			};
		}

		/// <summary>
		/// builds view models out for applications
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public ApplicationInfoViewModel BuildApplications(Application app)
		{
			List<ApplicationDocumentViewModel> docs = new List<ApplicationDocumentViewModel>();
			foreach (ApplicationDocument doc in app.ApplicationDocuments)
			{
				docs.Add(new ApplicationDocumentViewModel()
				{
					ApplicationDocumentId = doc.ApplicationDocumentId,
					ApplicationId = doc.ApplicationId,
					DocumentLink = doc.DocumentLink,
					DocumentName = doc.DocumentName
				});
			}

			return new ApplicationInfoViewModel()
			{
				ApplicationId = app.ApplicationId,
				ApplicantName = app.Applicant.LastName + ", " + app.Applicant.FirstName,
				ApplicantAddress = app.Applicant.City + ", " + app.Applicant.State + " " + app.Applicant.Country,
				ApplicantEmail = app.Applicant.Email,
				AppliationStatusId = (int)app.ApplicationStatus,
				ApplicationModifiedUTC = app.ApplicationModifiedUtc,
				Notes = app.Notes,
				ApplicationDocuments = docs
			};
		}
	}
}