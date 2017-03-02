﻿//------------------------------------------------------------------------------
// <copyright file="CreateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Customer;
using System.Linq;
using System.Web.Mvc;

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
		/// <returns>Presents a page for the creation of a new Customer.</returns>
		[HttpGet]
		public ActionResult Create()
		{
			if (Service.Can(Actions.CoreAction.EditCustomer))
			{
				var idAndCountries = Service.GetNextCustIdAndCountries();

				return this.View(new EditCustomerInfoViewModel
				{
					ValidCountries = idAndCountries.Item2,
					IsCreating = true,
					CustomerOrgId = idAndCountries.Item1
				});
			}

			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction(ActionConstants.Index);
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
				// CustomerOrgId must be unique
				if (Service.GetCustomerList(this.UserContext.ChosenOrganizationId).Any(customer => customer.CustomerOrgId == model.CustomerOrgId))
				{
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.CustomerOrgIdNotUnique, Variety.Danger));
					return this.View(model);
				}

				int? customerId = Service.CreateCustomer(new CustomerInfo()
				{
					ContactEmail = model.ContactEmail,
					Name = model.Name,
					Address = model.Address,
					City = model.City,
					State = model.State,
					Country = model.Country,
					PostalCode = model.PostalCode,
					ContactPhoneNumber = model.ContactPhoneNumber,
					FaxNumber = model.FaxNumber,
					Website = model.Website,
					EIN = model.EIN,
					OrganizationId = this.UserContext.ChosenOrganizationId,
					CustomerOrgId = model.CustomerOrgId
				});

				if (customerId.HasValue)
				{
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.CustomerCreatedNotification, Variety.Success));

					// Redirect to the user details page
					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index), customerId));
				}

				// No customer value, should only happen because of a permission failure
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));

				return this.RedirectToAction(ActionConstants.Index);
			}

			// Invalid model
			return this.View(model);
		}
	}
}
