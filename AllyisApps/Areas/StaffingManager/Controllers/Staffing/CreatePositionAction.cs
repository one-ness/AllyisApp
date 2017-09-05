//------------------------------------------------------------------------------
// <copyright file="CreatePositionAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Lookup;
using AllyisApps.ViewModels.Staffing;
using AllyisApps.Services.StaffingManager;
using System.Web.Mvc;
using AllyisApps.ViewModels;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;

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
			UserSubscription subInfo = null;
			this.AppService.UserContext.UserSubscriptions.TryGetValue(subscriptionId, out subInfo);

			var idAndCountries = AppService.GetNextCustId(subscriptionId);
			string subscriptionNameToDisplay = AppService.UserContext.UserSubscriptions[subscriptionId].SubscriptionName;
			//TODO: this is piggy-backing off the get index action, create a new action that just gets items 3-5.
			var infos = AppService.GetStaffingIndexInfo(subInfo.OrganizationId);
			return new EditPositionViewModel
			{
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService),

				IsCreating = true,
				OrganizationId = subInfo.OrganizationId,
				SubscriptionName = subscriptionNameToDisplay,
				SubscriptionId = subInfo.SubscriptionId,
				EmploymentTypes = infos.Item3,
				PositionLevels = infos.Item4,
				PositionStatuses = infos.Item5
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
				int? customerId = AppService.CreatePosition(
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
						BillingRateFrequency = model.BillingRateFrequency,
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

						Tags = model.Tags
					});

				if (customerId.HasValue)
				{
					// CustomerOrgId is not unique
					if (customerId == -1)
					{
						Notifications.Add(new BootstrapAlert(Resources.Strings.CustomerOrgIdNotUnique, Variety.Danger));
						return this.View(model);
					}

					Notifications.Add(new BootstrapAlert(Resources.Strings.CustomerCreatedNotification, Variety.Success));

					// Redirect to the user details page
					return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = subscriptionId });
				}

				// No customer value, should only happen because of a permission failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));

				return this.RedirectToAction(ActionConstants.Index);
			}

			// Invalid model
			return this.View(model);
		}
	}
}