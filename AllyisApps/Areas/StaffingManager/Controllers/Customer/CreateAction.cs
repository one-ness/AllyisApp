//------------------------------------------------------------------------------
// <copyright file="CreateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services.Crm;
using AllyisApps.Services.Lookup;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.Staffing.Customer;

namespace AllyisApps.Areas.StaffingManager.Controllers
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
		public async Task<ActionResult> Create(int subscriptionId)
		{
			var nextCustomerIdTask = AppService.GetNextCustId(subscriptionId);
			var subscriptionNameToDisplayTask = AppService.GetSubscriptionName(subscriptionId);

			await Task.WhenAll(new Task[] { nextCustomerIdTask, subscriptionNameToDisplayTask });

			var nextCustomerId = nextCustomerIdTask.Result;
			string subscriptionNameToDisplay = subscriptionNameToDisplayTask.Result;

			int orgID = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			return View(new EditCustomerInfoViewModel
			{
				LocalizedCountries = ModelHelper.GetLocalizedCountries(AppService),
				IsCreating = true,
				CustomerOrgId = nextCustomerId,
				SubscriptionId = subscriptionId,
				OrganizationId = orgID,
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
		public async Task<ActionResult> Create(EditCustomerInfoViewModel model)
		{
			if (ModelState.IsValid)
			{
				int? customerId = await AppService.CreateCustomerAsync(
					new Customer
					{
						ContactEmail = model.ContactEmail,
						CustomerName = model.CustomerName,
						Address = new Address
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
						return View(model);
					}

					Notifications.Add(new BootstrapAlert(Resources.Strings.CustomerCreatedNotification, Variety.Success));

					// Redirect to the user details page
					return RedirectToAction(ActionConstants.Index, new { subscriptionId = model.SubscriptionId });
				}

				// No customer value, should only happen because of a permission failure
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));

				return RedirectToAction(ActionConstants.Index);
			}

			// Invalid model
			return View(model);
		}
	}
}