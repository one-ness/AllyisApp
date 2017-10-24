//------------------------------------------------------------------------------
// <copyright file="EditOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Lookup;
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
		/// POST: Account/EditOrg.
		/// </summary>
		/// <param name="model">The organization ViewModel POST data.</param>
		/// <returns>Manage Page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		async public Task<ActionResult> EditOrg(EditOrganizationViewModel model)
		{
			if (ModelState.IsValid)
			{
				await this.AppService.UpdateOrganization(model.OrganizationId, model.OrganizationName, model.SiteUrl, model.AddressId, model.Address, model.City, model.SelectedStateId, model.SelectedCountryCode, model.PostalCode, model.PhoneNumber, model.FaxNumber, null);
				Notifications.Add(new BootstrapAlert(@Resources.Strings.OrganizationDetailsUpdated, Variety.Success));
				return this.RedirectToAction(ActionConstants.OrganizationDetails, ControllerConstants.Account, new { id = model.OrganizationId });
			}

			// Model is invalid, try again
			return this.View(model);
		}

		/// <summary>
		/// GET: /Account/EditOrg.
		/// The page for editing an organization's information.
		/// </summary>
		/// <param name="id">The organization id.</param>
		[HttpGet]
		async public Task<ActionResult> EditOrg(int id)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
			bool canDelete = this.AppService.CheckOrgAction(AppService.OrgAction.DeleteOrganization, id, false);
			var organization = await this.AppService.GetOrganization(id);
			var model = new EditOrganizationViewModel()
			{
				OrganizationId = organization.OrganizationId,
				OrganizationName = organization.OrganizationName,
				SiteUrl = organization.SiteUrl,
				AddressId = organization.Address?.AddressId,
				Address = organization.Address?.Address1,
				City = organization.Address?.City,
				SelectedStateId = organization.Address?.StateId,
				SelectedCountryCode = organization.Address?.CountryCode,
				Country = organization.Address?.CountryName,
				PostalCode = organization.Address?.PostalCode,
				PhoneNumber = organization.PhoneNumber,
				FaxNumber = organization.FaxNumber,
				CanDelete = canDelete,
				EmployeeId = "value",//Value needed for model TODO: Seperate Edit and Create
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService),
				LocalizedStates = ModelHelper.GetLocalizedStates(this.AppService, organization.Address?.CountryCode ?? string.Empty)
			};

			return this.View(model);
		}
	}
}