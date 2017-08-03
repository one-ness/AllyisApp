//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Customer;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Represents pages for the management of a Customer.
	/// </summary>
	public partial class CustomerController : BaseController
	{
		/// <summary>
		/// GET: Customer/{SubscriptionId}/Edit.
		/// </summary>
		/// <param name="userId">The Customer id.</param>
		/// <param name="subscriptionId">The Subscription Id</param>
		/// <returns>Presents a page to edit Customer data.</returns>
		[HttpGet]
		public ActionResult Edit(int subscriptionId, int userId)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditCustomer, subscriptionId);
			var infos = AppService.GetCustomerAndCountries(userId);

			return this.View(new EditCustomerInfoViewModel
			{
				ContactEmail = infos.Item1.ContactEmail,
                CustomerName = infos.Item1.CustomerName,
				AddressId = infos.Item1.AddressId,
				Address = infos.Item3.Address1,
				City = infos.Item3.City,
				State = infos.Item3.State,
				Country = infos.Item3.CountryId,
				PostalCode = infos.Item3.PostalCode,
				ContactPhoneNumber = infos.Item1.ContactPhoneNumber,
				FaxNumber = infos.Item1.FaxNumber,
				Website = infos.Item1.Website,
				EIN = infos.Item1.EIN,
				OrganizationId = infos.Item1.OrganizationId,
				CustomerId = userId,
				ValidCountries = infos.Item2,
				CustomerOrgId = infos.Item1.CustomerOrgId,
				SubscriptionId = subscriptionId
			});
		}

		/// <summary>
		/// POST: Customer/Edit.
		/// </summary>
		/// <param name="model">The Customer view model.</param>
		/// <returns>The ActionResult.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditCustomerInfoViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = AppService.UpdateCustomer(new Customer()
				{
					CustomerId = model.CustomerId,
					ContactEmail = model.ContactEmail,
                    CustomerName = model.CustomerName,
					AddressId = model.AddressId,
					Address = model.Address,
					City = model.City,
					State = model.State,
					Country = model.Country,
					PostalCode = model.PostalCode,
					ContactPhoneNumber = model.ContactPhoneNumber,
					FaxNumber = model.FaxNumber,
					Website = model.Website,
					EIN = model.EIN,
					CustomerOrgId = model.CustomerOrgId,
					OrganizationId = model.OrganizationId
				}, model.SubscriptionId);

				if (result == -1)   //the new CustOrgId is not unique
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.CustomerOrgIdNotUnique, Variety.Danger));
					return this.View(model);
				}
				else if (result == 1) //updated successfully
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.CustomerDetailsUpdated, Variety.Success));
					return this.Redirect(string.Format("{0}#customerNumber{1}", Url.Action(ActionConstants.Index, new { subscriptionId = model.SubscriptionId }), model.CustomerId));
				}
				else // Permissions failure
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
					return this.RedirectToAction(ActionConstants.Index, new { subscriptionId = model.SubscriptionId });
				}
			}

			// Invalid model
			return this.View(model);
		}
	}
}
