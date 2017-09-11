//------------------------------------------------------------------------------
// <copyright file="CreatePositionAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.StaffingManager;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.Staffing;
using System.Web.Mvc;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Represents pages for the management of a position.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// GET: Position/Create.
		/// </summary>
		/// <param name="subscriptionId">The subscription.</param>
		/// <param name="organizationId">The organization.</param>
		/// <returns>Presents a page for the creation of a new Position.</returns>
		[HttpGet]
		public ActionResult Create(int subscriptionId, int organizationId)
		{
			// var idAndCountries = AppService.GetNextCustId(subscriptionId);
			string subscriptionNameToDisplay = AppService.getSubscriptionName(subscriptionId);
			return this.View(new CreatePositionViewModel
			{
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService),

				IsCreating = true,

				// SubscriptionId = subscriptionId,
				OrganizationId = organizationId,
				SubscriptionName = subscriptionNameToDisplay,
			});
		}

		/// <summary>
		/// POST: Position/Create.
		/// </summary>
		/// <param name="model">The Position ViewModel.</param>
		/// <returns>The resulting page, Create if unsuccessful else Position Index.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreatePositionViewModel model)
		{
			if (ModelState.IsValid)
			{
				int? positionId = AppService.CreatePosition(
					new Position()
					{
						OrganizationId = model.OrganizationId,
						CustomerId = model.CustomerId,
						Address = new Address()
						{
							Address1 = model.Address,
							City = model.City,
							StateName = model.State,
							CountryName = model.Country,
							PostalCode = model.PostalCode,
							CountryCode = model.SelectedCountryCode,
							StateId = model.SelectedStateId
						},
						StartDate = model.StartDate,
						PositionStatusId = model.PositionStatusId,
						PositionTitle = model.PositionTitle,
						BillingRateAmount = model.BillingRateAmount,
						BillingRateFrequency = model.BillingRateFrequency,
						DurationMonths = model.DurationMonths,
						EmploymentTypeId = model.EmploymentTypeId,
						PositionCount = model.PositionCount,
						RequiredSkills = model.RequiredSkills,
						JobResponsibilities = model.JobResponsibilities,
						DesiredSkills = model.DesiredSkills,
						PositionLevelId = model.PositionLevelId,
						HiringManager = model.HiringManager,
						TeamName = model.TeamName
					});

				if (positionId.HasValue)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.CustomerCreatedNotification, Variety.Success));

					// Redirect to the position details page
					return this.RedirectToAction(ActionConstants.Index, new { positionId = positionId });
				}

				// No position value, should only happen because of a permission failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));

				return this.RedirectToAction(ActionConstants.Index);
			}

			// Invalid model
			return this.View(model);
		}
	}
}