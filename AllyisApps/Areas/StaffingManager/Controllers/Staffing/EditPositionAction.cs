//------------------------------------------------------------------------------
// <copyright file="CreatePositionAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Lookup;
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
		public async Task<ActionResult> EditPosition(int positionId, int subscriptionId)
		{
			SetNavData(subscriptionId);

			var editModel = await setupEditPositionViewModel(positionId, subscriptionId);

			return View(editModel);
		}

		/// <summary>
		/// setup position setup viewmodel
		/// </summary>
		/// <returns></returns>
		public async Task<EditPositionViewModel> setupEditPositionViewModel(int positionId, int subscriptionId)
		{
			UserContext.SubscriptionAndRole subInfo = null;
			AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			Position pos = await AppService.GetPosition(positionId);

			var subscriptionNameToDisplayTask = AppService.GetSubscriptionName(subscriptionId);
			//TODO: this is piggy-backing off the get index action, create a new action that just gets items 3-5.
			var infosTask = AppService.GetStaffingIndexInfo(subInfo.OrganizationId);

			await Task.WhenAll(new Task[] { infosTask, subscriptionNameToDisplayTask });

			string subscriptionNameToDisplay = subscriptionNameToDisplayTask.Result;
			var infos = infosTask.Result;

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
			foreach (var tag in pos.Tags) assignedTags += "," + tag.TagName;
			return new EditPositionViewModel
			{
				PositionId = pos.PositionId,
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService.GetCountries()),
				LocalizedStates = ModelHelper.GetLocalizedStates(this.AppService.GetStates(pos.Address.CountryCode)),
				IsCreating = false,
				OrganizationId = subInfo.OrganizationId,
				SubscriptionName = subscriptionNameToDisplay,
				SubscriptionId = subInfo.SubscriptionId,
				StartDate = pos.StartDate,
				Tags = tags,
				EmploymentTypes = infos.Item3.AsParallel().Select(et => new EmploymentTypeSelectViewModel
				{
					EmploymentTypeId = et.EmploymentTypeId,
					EmploymentTypeName = et.EmploymentTypeName
				}).ToList(),
				PositionLevels = infos.Item4.AsParallel().Select(pl => new PositionLevelSelectViewModel
				{
					PositionLevelId = pl.PositionLevelId,
					PositionLevelName = pl.PositionLevelName
				}).ToList(),
				PositionStatuses = infos.Item5.AsParallel().Select(ps => new PositionStatusSelectViewModel
				{
					PositionStatusId = ps.PositionStatusId,
					PositionStatusName = ps.PositionStatusName
				}).ToList(),
				Customers = infos.Item7.AsParallel().Select(cus => new CustomerSelectViewModel
				{
					CustomerId = cus.CustomerId,
					CustomerName = cus.CustomerName
				}).ToList(),
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
				},
				SelectedCountryCode = pos.Address?.CountryCode,
				SelectedStateId = pos.Address?.StateId
			};
		}

		/// <summary>
		/// POST: Customer/Create.
		/// </summary>
		/// <param name="model">The Customer ViewModel.</param>
		/// <param name="subscriptionId">The sub id from the ViewModel.</param>
		/// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
		public async Task<ActionResult> SubmitUpdatePosition(EditPositionViewModel model, int subscriptionId)
		{
			// Invalid model TODO: redirect back to form page
			if (!ModelState.IsValid)
			{
				return RedirectToAction(ActionConstants.CreatePosition, new { subscriptionId });
			}

			var tags = new List<Tag>();
			if (model.OrganizationId == 0)
			{
				AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out UserContext.SubscriptionAndRole subInfo);
				model.OrganizationId = subInfo.OrganizationId;
				if (model.TagsToSubmit != null)
				{
					var tagArray = new JavaScriptSerializer().Deserialize<string[]>(model.TagsToSubmit);

					tags = tagArray.Select(tag => new Tag
					{
						TagName = tag == "" ? "New" : tag,
						TagId = -1,
						PositionId = -1
					}).ToList();
				}
				if (model.PositionStatusId == 0)
				{
					var defaultGet = await AppService.GetStaffingDefaultStatus(subInfo.SubscriptionId);
					model.PositionStatusId = defaultGet[0];
				}
			}
			int? positionId = await AppService.UpdatePosition(
				new Position
				{
					OrganizationId = model.OrganizationId,
					CustomerId = model.CustomerId,
					AddressId = model.AddressId,
					PositionStatusId = model.PositionStatusId,
					PositionTitle = model.PositionTitle,
					DurationMonths = model.DurationMonths,
					EmploymentTypeId = model.EmploymentTypeId,
					PositionCount = model.PositionCount,
					RequiredSkills = model.RequiredSkills,
					PositionLevelId = model.PositionLevelId,
					PositionId = model.PositionId,
					PositionCreatedUtc = model.PositionCreatedUtc,
					PositionModifiedUtc = model.PositionModifiedUtc,
					StartDate = model.StartDate,
					BillingRateFrequency = (int)model.BillingRateFrequency,
					BillingRateAmount = model.BillingRateAmount,
					JobResponsibilities = model.JobResponsibilities,
					DesiredSkills = model.DesiredSkills,
					HiringManager = model.HiringManager,
					TeamName = model.TeamName,

					Address = new Address
					{
						Address1 = model.PositionAddress.Address,
						City = model.PositionAddress.City,
						StateId = model.PositionAddress.SelectedStateId,
						CountryCode = model.PositionAddress.SelectedCountryCode,
						PostalCode = model.PositionAddress.PostalCode
					},

					Tags = tags
				});

			if (positionId.HasValue)
			{
				Notifications.Add(new BootstrapAlert("Successfully created a new Position", Variety.Success));

				// Redirect to the user position page
				return RedirectToAction(ActionConstants.Index, new { subscriptionId });
			}

			// No customer value, should only happen because of a permission failure
			Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));

			return RedirectToAction(ActionConstants.Index);
		}
	}
}