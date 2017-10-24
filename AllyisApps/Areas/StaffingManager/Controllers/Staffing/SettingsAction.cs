//------------------------------------------------------------------------------
// <copyright file="SettingsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Crm;
using AllyisApps.Services.StaffingManager;
using AllyisApps.ViewModels;
using System.Threading.Tasks;

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
		async public Task<ActionResult> Settings(int subscriptionId)
		{
			SetNavData(subscriptionId);

			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			string subscriptionNameToDisplay = await AppService.GetSubscriptionName(subscriptionId);
			var defaultStatus = await AppService.GetStaffingDefaultStatus(subInfo.OrganizationId); //[0] is default position status, [1] is default application status

			int? defaultPosStat = null;
			if (defaultStatus.Count >= 1) defaultPosStat = defaultStatus[0];
			int? defaultAppStat = null;
			if (defaultStatus.Count >= 2) defaultAppStat = defaultStatus[1];

			var infos = await AppService.GetStaffingIndexInfo(subInfo.OrganizationId);

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
				infos.Item6,
				infos.Item7,
				defaultPosStat, //positionstatusdefault
				defaultAppStat  //applicationStatusdefault
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
		/// <param name="applicationStatuses"></param>
		/// <param name="customers"></param>
		/// <param name="defaultStatusId"></param>
		/// <param name="defaultAppStatusId"></param>
		/// <returns></returns>
		public StaffingSettingsViewModel ConstructStaffingSettingsViewModel(int orgId, int subId, string subName,
						List<Services.Lookup.Tag> tags, List<EmploymentType> employmentTypes, List<PositionLevel> positionLevelsList,
						List<PositionStatus> positionStatuses, List<ApplicationStatus> applicationStatuses, List<Customer> customers, int? defaultStatusId, int? defaultAppStatusId)
		{
			StaffingSettingsViewModel result = new StaffingSettingsViewModel()
			{
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService),
				organizationId = orgId,
				subscriptionId = subId,
				subscriptionName = subName,
				tags = tags.AsParallel().Select(t => new TagViewModel() { TagId = t.TagId, TagName = t.TagName, PositionId = t.PositionId }).ToList(),
				employmentTypes = employmentTypes.AsParallel().Select(empt => new EmploymentTypeSelectViewModel() { EmploymentTypeId = empt.EmploymentTypeId, EmploymentTypeName = empt.EmploymentTypeName }).ToList(),
				positionLevels = positionLevelsList.AsParallel().Select(pos => new PositionLevelSelectViewModel() { PositionLevelId = pos.PositionLevelId, PositionLevelName = pos.PositionLevelName }).ToList(),
				positionStatuses = positionStatuses.AsParallel().Select(pos => new PositionStatusSelectViewModel() { PositionStatusId = pos.PositionStatusId, PositionStatusName = pos.PositionStatusName }).ToList(),
				applicationStatuses = applicationStatuses.AsParallel().Select(pos => new ApplicationStatusSelectViewModel() { ApplicationStatusId = pos.ApplicationStatusId, ApplicationStatusName = pos.ApplicationStatusName }).ToList(),
				customers = customers,
				defaultPositionStatus = defaultStatusId,
				defaultApplicationStatus = defaultAppStatusId
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
			if (string.IsNullOrWhiteSpace(positionLevel))
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
		/// update status default for the users Org
		/// </summary>
		/// <param name="organizationId"></param>
		/// <param name="positionStatusId"></param>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public ActionResult updatePositionStatus(int organizationId, int positionStatusId, int subscriptionId)
		{
			try
			{
				AppService.UpdateDefaultPositionStatus(organizationId, positionStatusId);
				Notifications.Add(new BootstrapAlert("update new Position Status", Variety.Success));
			}
			catch (ArgumentException)
			{
				// Level already exists
				Notifications.Add(new BootstrapAlert("Position Status already exists", Variety.Danger));
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}

		///// <summary>
		///// POST:
		///// </summary>
		///// <param name="model">The settings ViewModel.</param>
		////// <returns>The resulting page, Create if unsuccessful else staffing settings.</returns>
		/*
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
							Address1 = model.newCustomerAddress.Address,
							City = model.newCustomerAddress.City,
							StateName = model.newCustomerAddress.State,
							CountryName = model.newCustomerAddress.Country,
							PostalCode = model.newCustomerAddress.PostalCode,
							CountryCode = model.newCustomerAddress.SelectedCountryCode,
							StateId = model.newCustomerAddress.SelectedStateId
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
		*/
	}
}