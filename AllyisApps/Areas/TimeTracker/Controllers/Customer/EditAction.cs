//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
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
		/// GET: Customer/Edit.
		/// </summary>
		/// <param name="id">The Customer id.</param>
		/// <returns>Presents a page to edit Customer data.</returns>
		[HttpGet]
		public ActionResult Edit(int id)
		{
			if (Service.Can(Actions.CoreAction.EditCustomer))
			{
				var infos = Service.GetCustomerAndCountries(id);
				
				return this.View(new EditCustomerInfoViewModel
				{
					ContactEmail = infos.Item1.ContactEmail,
					Name = infos.Item1.Name,
					Address = infos.Item1.Address,
					City = infos.Item1.City,
					State = infos.Item1.State,
					Country = infos.Item1.Country,
					PostalCode = infos.Item1.PostalCode,
					ContactPhoneNumber = infos.Item1.ContactPhoneNumber,
					FaxNumber = infos.Item1.FaxNumber,
					Website = infos.Item1.Website,
					EIN = infos.Item1.EIN,
					OrganizationId = infos.Item1.OrganizationId,
					CustomerID = id,
					ValidCountries = infos.Item2,
					CustomerOrgId = infos.Item1.CustomerOrgId
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
				// CustomerOrgId must be unique, if it has been changed
				Customer orgIdMatch = Service.GetCustomerList(this.UserContext.ChosenOrganizationId).Where(customer => customer.CustomerOrgId == model.CustomerOrgId).SingleOrDefault();
				if (orgIdMatch != null && orgIdMatch.CustomerId != model.CustomerID)
				{
					Notifications.Add(new BootstrapAlert(Resources.TimeTracker.Controllers.Customer.Strings.CustomerOrgIdNotUnique, Variety.Danger));
					return this.View(model);
				}

				if (Service.UpdateCustomer(new Customer()
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
