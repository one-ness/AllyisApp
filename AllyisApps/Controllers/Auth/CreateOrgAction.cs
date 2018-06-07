//------------------------------------------------------------------------------
// <copyright file="CreateOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers.Auth
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
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService.GetCountries());

			return View(model);
		}

		/// <summary>
		/// POST: Create an organization.
		/// </summary>
		/// <param name="model">The organization ViewModel.</param>
		/// <returns>The resulting page, Create if unsuccessful else Account Management.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateOrg(EditOrganizationViewModel model)
		{
			if (ModelState.IsValid)
			{
				int orgId = await AppService.SetupNewOrganization(model.EmployeeId, model.OrganizationName, model.PhoneNumber, model.FaxNumber, model.SiteUrl, model.Subdomain, model.Address, model.City, model.SelectedStateId, model.PostalCode, model.SelectedCountryCode);

				if (orgId <= 0)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SubdomainTaken, Variety.Danger));
					return View(model);
				}
				else
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.OrganizationCreatedNotification, Variety.Success));
					return RedirectToAction(ActionConstants.Skus, ControllerConstants.Account, new { id = orgId });
				}
			}

			// Something happened, reload this view
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService.GetCountries());
			model.LocalizedStates = ModelHelper.GetLocalizedStates(this.AppService.GetStates(model.SelectedCountryCode));
			return View(model);
		}
	}
}