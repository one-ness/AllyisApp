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
using AllyisApps.Services.StaffingManager;
using System.Collections.Generic;
using AllyisApps.Core.Alert;
using System;
using AllyisApps.ViewModels;
using AllyisApps.Services.Lookup;

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
			string subscriptionNameToDisplay = AppService.getSubscriptionName(subscriptionId);

			var infos = AppService.GetStaffingIndexInfo(subInfo.OrganizationId);

			//ViewBag.SignedInUserID = GetCookieData().UserId;
			//ViewBag.SelectedUserId = userId;

			StaffingSettingsViewModel model = this.ConstructStaffingSettingsViewModel(
				subInfo.OrganizationId,
				subscriptionId,
				subscriptionNameToDisplay,
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
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService),
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

		/// <summary>
		/// Create new position level for the users Org
		/// </summary>
		/// <param name="positionLevel"></param>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public ActionResult CreatePositionLevel(string positionLevel, int subscriptionId)
		{
			if(string.IsNullOrWhiteSpace(positionLevel))
			{
				Notifications.Add(new BootstrapAlert("Position Level cannot be blank", Variety.Warning));
			}
			else
			{
				int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

				// should put try catch in 'else'. Creating a blank level results in Two alerts: "Cannot create blank position level" and "pay class already exists"
				try
				{
					AppService.CreatePositionLevel(positionLevel, orgId, subscriptionId);
					Notifications.Add(new BootstrapAlert("Created new Position Level", Variety.Success));
				}
				catch (ArgumentException)
				{
					// Level already exists
					Notifications.Add(new BootstrapAlert("Position Level already exists", Variety.Danger));
				}
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}

		/// <summary>
		/// Create new position level for the users Org
		/// </summary>
		/// <param name="positionStatus"></param>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public ActionResult CreatePositionStatus(string positionStatus, int subscriptionId)
		{
			if (string.IsNullOrWhiteSpace(positionStatus))
			{
				Notifications.Add(new BootstrapAlert("Position Status cannot be blank", Variety.Warning));
			}
			else
			{
				int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

				// should put try catch in 'else'. Creating a blank level results in Two alerts: "Cannot create blank position level" and "pay class already exists"
				try
				{
					AppService.CreatePositionStatus(positionStatus, orgId, subscriptionId);
					Notifications.Add(new BootstrapAlert("Created new Position Status", Variety.Success));
				}
				catch (ArgumentException)
				{
					// Level already exists
					Notifications.Add(new BootstrapAlert("Position Status already exists", Variety.Danger));
				}
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}

		/// <summary>
		/// Create new position level for the users Org
		/// </summary>
		/// <param name="employmentType"></param>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public ActionResult CreateEmploymentType(string employmentType, int subscriptionId)
		{
			if (string.IsNullOrWhiteSpace(employmentType))
			{
				Notifications.Add(new BootstrapAlert("Position Status cannot be blank", Variety.Warning));
			}
			else
			{
				int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

				// should put try catch in 'else'. Creating a blank level results in Two alerts: "Cannot create blank position level" and "pay class already exists"
				try
				{
					AppService.CreateEmploymentType(employmentType, orgId, subscriptionId);
					Notifications.Add(new BootstrapAlert("Created new Position Status", Variety.Success));
				}
				catch (ArgumentException)
				{
					// Level already exists
					Notifications.Add(new BootstrapAlert("Position Status already exists", Variety.Danger));
				}
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}


		/// <summary>
		/// POST: 
		/// </summary>
		/// <param name="model">The settings ViewModel.</param>
		/// <returns>The resulting page, Create if unsuccessful else staffing settings.</returns>
		public ActionResult CreateCustomer(StaffingSettingsViewModel model)
		{
			if (ModelState.IsValid)
			{

				UserContext.SubscriptionAndRole subInfo = null;
				this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(model.subscriptionId, out subInfo);
				int? customerId = AppService.CreateStaffingCustomer(
					new Customer()
					{
						ContactEmail = model.newCustomer.ContactEmail,
						CustomerName = model.newCustomer.CustomerName,
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
						ContactPhoneNumber = model.newCustomer.ContactPhoneNumber,
						FaxNumber = model.newCustomer.FaxNumber,
						Website = model.newCustomer.Website,
						EIN = model.newCustomer.EIN,
						OrganizationId = subInfo.OrganizationId,
						CustomerOrgId = model.newCustomer.CustomerOrgId
					},
					model.subscriptionId);

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
					return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = model.subscriptionId });
				}

				// No customer value, should only happen because of a permission failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));

				return this.RedirectToAction(ActionConstants.Settings);
			}

			// Invalid model
			return this.View(model);
		}
	}
}