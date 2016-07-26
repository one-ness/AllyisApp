//------------------------------------------------------------------------------
// <copyright file="CreateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.ViewModels;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseProductController
	{
		/// <summary>
		/// GET: Customer/Create.
		/// </summary>
		/// <returns>Presents a page for the creation of a new Customer.</returns>
		[HttpGet]
		public ActionResult Create()
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditCustomer))
			{
				return this.View(new EditCustomerInfoViewModel
				{
					ValidCountries = AccountService.ValidCountries()
				});
			}

			Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));
			return this.RedirectToAction("Index");
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
				int? customerId = CrmService.CreateCustomer(new CustomerInfo()
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
					OrganizationId = this.UserContext.ChosenOrganizationId
				});

				if (customerId.HasValue)
				{
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.CustomerCreatedNotification, Variety.Success));

					// Redirect to the user details page
					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action("Index"), customerId));
				}

				// No customer value, should only happen because of a permission failure
				Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.ActionUnauthorizedMessage, Variety.Warning));

				return this.RedirectToAction("Index");
			}

			// Invalid model
			return this.View(model);
		}
	}
}