//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Customer;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController: BaseController
	{
		/// <summary>
		/// GET: Customer/Edit.
		/// </summary>
		/// <param name="id">The Customer id.</param>
		/// <returns>Presents a page to edit Customer data.</returns>
		[HttpGet]
		public ActionResult Edit(int id)
		{
			if (Service.Can(Actions.CoreAction.EditCustomer))
			{
				CustomerInfo customer = Service.GetCustomer(id);
				return this.View(new EditCustomerInfoViewModel
				{
					ContactEmail = customer.ContactEmail,
					Name = customer.Name,
					Address = customer.Address,
					City = customer.City,
					State = customer.State,
					Country = customer.Country,
					PostalCode = customer.PostalCode,
					ContactPhoneNumber = customer.ContactPhoneNumber,
					FaxNumber = customer.FaxNumber,
					Website = customer.Website,
					EIN = customer.EIN,
					OrganizationId = customer.OrganizationId,
					OrganizationName = Service.GetOrganization(customer.OrganizationId).Name,
					CustomerID = id,
					ValidCountries = Service.ValidCountries(),
                    CustomerOrgId = customer.CustomerOrgId
				});
			}

			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));

			return this.RedirectToAction(ActionConstants.Index);
		}

		/// <summary>
		/// POST: Customer/Edit.
		/// </summary>
		/// <param name="model">The Customer view model.</param>
		/// <returns>The ActionResult.</returns>
		[HttpPost]
		public ActionResult Edit(EditCustomerInfoViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (Service.UpdateCustomer(new CustomerInfo()
				{
					CustomerId = model.CustomerID,
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
                    CustomerOrgId = model.CustomerOrgId
				}))
				{
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.CustomerDetailsUpdated, Variety.Success));
					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index), model.CustomerID));
				}

				// Permissions failure
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));
				return this.RedirectToAction(ActionConstants.Index);
			}

			// Invalid model
			return this.View(model);
		}
	}
}