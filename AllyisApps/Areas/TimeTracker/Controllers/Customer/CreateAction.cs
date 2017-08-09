//------------------------------------------------------------------------------
// <copyright file="CreateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
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
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.CreateCustomer, subscriptionId);
			var idAndCountries = AppService.GetNextCustIdAndCountries(subscriptionId);
			string subscriptionNameToDisplay = AppService.UserContext.OrganizationSubscriptions[subscriptionId].SubscriptionName;
			return this.View(new EditCustomerInfoViewModel
			{
				ValidCountries = idAndCountries.Item2,
				IsCreating = true,
				CustomerOrgId = idAndCountries.Item1,
				SubscriptionId = subscriptionId,
				OrganizationId = idAndCountries.Item3,
				UserId = AppService.UserContext.UserId,
				SubscriptionName = subscriptionNameToDisplay
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
						Address = model.Address,
						City = model.City,
						State = model.State,
						Country = model.Country,
						PostalCode = model.PostalCode,
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
