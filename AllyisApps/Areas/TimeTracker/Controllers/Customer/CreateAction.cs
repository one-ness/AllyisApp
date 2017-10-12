//------------------------------------------------------------------------------
// <copyright file="CreateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Lookup;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.TimeTracker.Customer;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// GET: Customer/Create.
		/// </summary>
		/// <param name="subscriptionId">The subscription.</param>
		/// <returns>Presents a page for the creation of a new Customer.</returns>
		[HttpGet]
		public ActionResult Create(int subscriptionId)
		{
			if (AppService.UserContext.SubscriptionsAndRoles[subscriptionId].ProductId != ProductIdEnum.StaffingManager)
			{
				this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditCustomer, subscriptionId);
			}
			int orgId = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

			var NextCustomerId = AppService.GetNextCustId(subscriptionId);
			string subscriptionNameToDisplay = AppService.GetSubscriptionName(subscriptionId);
			return this.View(new EditCustomerInfoViewModel
			{
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService),
				IsCreating = true,
				CustomerOrgId = NextCustomerId,
				SubscriptionId = subscriptionId,
				OrganizationId = orgId,
				UserId = AppService.UserContext.UserId,
				SubscriptionName = subscriptionNameToDisplay,
			});
		}

		/// <summary>
		/// POST: Customer/Create.
		/// </summary>
		/// <param name="model">The Customer ViewModel.</param>
		/// <returns>The resulting page, Create if unsuccessful else Customer Index.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(EditCustomerInfoViewModel model)
		{
			if (ModelState.IsValid)
			{
				int? customerId = AppService.CreateCustomer(
					new Customer()
					{
						ContactEmail = model.ContactEmail,
						CustomerName = model.CustomerName,
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
						ContactPhoneNumber = model.ContactPhoneNumber,
						FaxNumber = model.FaxNumber,
						Website = model.Website,
						EIN = model.EIN,
						OrganizationId = model.OrganizationId,
						CustomerOrgId = model.CustomerOrgId
					},
					model.SubscriptionId);

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
					return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = model.SubscriptionId });
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