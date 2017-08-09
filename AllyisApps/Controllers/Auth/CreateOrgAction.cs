//------------------------------------------------------------------------------
// <copyright file="CreateOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using System.Web.Mvc;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get /Account/CreateOrg
		/// Create an organization page.
		/// </summary>
		/// <returns>Presents a page for the creation of a new organization.</returns>
		[HttpGet]
		public ActionResult CreateOrg()
		{
			var model = new EditOrganizationViewModel();
			model.IsCreating = true;

			// create localized countries
			var countries = this.AppService.GetCountries();
			foreach (var item in countries)
			{
				// get the country name
				string countryName = Utility.AggregateSpaces(item.Value);

				// use the country name in the resource file to get it's localized name
				string localized = Resources.Countries.ResourceManager.GetString(countryName) ?? item.Value;

				// add it to localized countries dictionary
				model.LocalizedCountries.Add(item.Key, localized);
			}

			return this.View(model);
		}

		/// <summary>
		/// POST: Create an organization.
		/// </summary>
		/// <param name="model">The organization ViewModel.</param>
		/// <returns>The resulting page, Create if unsuccessful else Account Management.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateOrg(EditOrganizationViewModel model)
		{
			if (ModelState.IsValid)
			{
				int orgId = AppService.SetupOrganization(
					new Organization()
					{
						Address = model.Address,
						City = model.City,
						Country = model.Country,
						OrganizationName = model.OrganizationName,
						SiteUrl = model.SiteUrl,
						State = model.State,
						PostalCode = model.PostalCode,
						PhoneNumber = model.PhoneNumber,
						FaxNumber = model.FaxNumber
					},
					model.EmployeeId);

				if (orgId < 0)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SubdomainTaken, Variety.Danger));
					return this.View(model);
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.OrganizationCreatedNotification, Variety.Success));
					return this.RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { organizationId = orgId });
				}
			}

			// Something happened, reload this view
			return this.View(model);
		}
	}
}
