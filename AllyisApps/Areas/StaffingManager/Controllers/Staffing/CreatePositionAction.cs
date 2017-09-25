//------------------------------------------------------------------------------
// <copyright file="CreatePositionAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;
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
		/// <param name="subscriptionId">The subscription.</param>
		/// <returns>Presents a page for the creation of a new position.</returns>
		public ActionResult CreatePosition(int subscriptionId)
		{
			var newmodel = setupPositionEditViewModel(subscriptionId);

			return this.View(newmodel);
		}

		/// <summary>
		/// setup position setup viewmodel
		/// </summary>
		/// <returns></returns>
		public EditPositionViewModel setupPositionEditViewModel(int subscriptionId)
		{
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);

			
			string subscriptionNameToDisplay = AppService.getSubscriptionName(subscriptionId);
			//TODO: this is piggy-backing off the get index action, create a new action that just gets items 3-5.
			var infos = AppService.GetStaffingIndexInfo(subInfo.OrganizationId);
			var temp = new string[infos.Item2.Count];
			var count = 0;
			for (int i = 0; i < infos.Item2.Count; i++)
			{
				bool taken = false;
				for (int j = 0; j < i; j++)
				{
					if (infos.Item2[i].TagName == temp[j] && !taken) taken = true;
					if (!taken) temp[count] = infos.Item2[i].TagName;
				}
			}
			var tags = new string[count];
			for (int k = 0; k < count; k++) tags[k] = temp[k];
			return new EditPositionViewModel
			{
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService),

				IsCreating = true,
				OrganizationId = subInfo.OrganizationId,
				SubscriptionName = subscriptionNameToDisplay,
				SubscriptionId = subInfo.SubscriptionId,
				StartDate = System.DateTime.UtcNow.Date,
				Tags = tags,
				EmploymentTypes = infos.Item3,
				PositionLevels = infos.Item4,
				PositionStatuses = infos.Item5,
				Customers = infos.Item6
			};
		}

		/// <summary>
		/// POST: Customer/Create.
		/// </summary>
		/// <param name="model">The Customer ViewModel.</param>
		/// <param name="subscriptionId">The sub id from the ViewModel.</param>
		/// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
		public ActionResult SubmitCreatePosition(EditPositionViewModel model, int subscriptionId)
		{
			if (ModelState.IsValid)
			{
				var tags = new List<Tag>();
				if (model.OrganizationId == 0)
				{
					UserContext.SubscriptionAndRole subInfo = null;
					this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
					model.OrganizationId = subInfo.OrganizationId;
					if (model.TagsToSubmit != null)
					{
						foreach (string tag in model.TagsToSubmit)
						{
							if (tag == "") tags.Add(new Tag { TagName = "New", TagId = -1, PositionId = -1 });
							else tags.Add(new Tag { TagName = tag, TagId = -1, PositionId = -1 });
						}
					}
				}
				int? positionId = AppService.CreatePosition(
					new Position()
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
							Address1 = model.Address,
							City = model.City,
							StateId = model.SelectedStateId,
							CountryCode = model.SelectedCountryCode,
							PostalCode = model.PostalCode
						},

						Tags = tags
					});

				if (positionId.HasValue)
				{
					Notifications.Add(new BootstrapAlert("Successfully created a new Position", Variety.Success));

					// Redirect to the user position page
					return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
				}

				// No customer value, should only happen because of a permission failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));

				return this.RedirectToAction(ActionConstants.Index);
			}

			// Invalid model TODO: redirect back to form page
			return this.RedirectToAction(ActionConstants.CreatePosition, new { subscriptionId = subscriptionId });
		}
	}
}