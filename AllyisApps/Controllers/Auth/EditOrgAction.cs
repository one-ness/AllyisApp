//------------------------------------------------------------------------------
// <copyright file="EditOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Lookup;
using AllyisApps.ViewModels;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers
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
		public ActionResult EditOrg(EditOrganizationViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (AppService.UpdateOrganization(
						BuildEditOrganizaiton(model)))
					{
						// Organization updated successfully
						Notifications.Add(new BootstrapAlert(@Resources.Strings.OrganizationDetailsUpdated, Variety.Success));
						return this.RedirectToAction(ActionConstants.ManageOrg, ControllerConstants.Account, new { id = model.OrganizationId });
					}
				}
				catch (ArgumentException)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SubdomainTaken, Variety.Danger));
					return this.RedirectToAction(ActionConstants.Edit, ControllerConstants.Account, new { id = model.OrganizationId });
				}

				// Organization update failed due to invalid permissions
				return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Strings.CannotEditProfileMessage), ControllerConstants.Organization, ActionConstants.Edit));
			}

			// Model is invalid, try again
			return this.View(model);
		}

		/// <summary>
		/// GET: /Account/EditOrg.
		/// The page for editing an organization's information.
		/// </summary>
		/// <param name="id">The organization id.</param>
		/// <param name="returnUrl">The return url to redirect to after form submit.</param>
		/// <returns>The result of this action.</returns>
		public ActionResult EditOrg(int id, string returnUrl)
		{
			this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);
			var infos = AppService.GetOrgWithCountriesAndEmployeeId(id);
			EditOrganizationViewModel model = this.ConstructEditOrganizationViewModel(infos.Item1, true, infos.Item2);
			model.EmployeeId = infos.Item3;
			ViewBag.returnUrl = returnUrl;
			return this.View(model);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EditOrganizationViewModel"/> class.
		/// </summary>
		/// <param name="organization">An <see cref="Organization"/> for the organization.</param>
		/// <param name="canDelete">The users permission to delete.</param>
		/// <param name="validCountries">List of valid countries.</param>
		/// <returns>An initialized EditOrganizationViewModel.</returns>
		public EditOrganizationViewModel ConstructEditOrganizationViewModel(Organization organization, bool canDelete, IEnumerable<string> validCountries)
		{
			var model = new EditOrganizationViewModel
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
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService)
			};
			model.LocalizedStates = ModelHelper.GetLocalizedStates(this.AppService, model.SelectedCountryCode);
			return model;
		}

		/// <summary>
		/// Builds an organizaion servie object form the view Model ensures that the built service object has all of its current propreties.
		/// </summary>
        /// <param name="model">An EditOrgViewModel.</param>
        /// <param name="loadOrginal">Sets if the model loads the original organization model.</param>
		/// <returns>Returns an Organization object.</returns>
		private Organization BuildEditOrganizaiton(EditOrganizationViewModel model, bool loadOrginal = false)
		{
			Organization orginal = null;

            if (loadOrginal)
			{
				orginal = AppService.GetOrganization(model.OrganizationId);
			}

			return new Organization()
			{
				OrganizationId = model.OrganizationId,
				OrganizationName = model.OrganizationName,
				SiteUrl = model.SiteUrl,
				Address = new Address()
				{
					AddressId = model.AddressId,
					City = model.City,
					StateId = model.SelectedStateId,
					CountryCode = model.SelectedCountryCode,
					PostalCode = model.PostalCode,
					Address1 = model.Address
				},

				PhoneNumber = model.PhoneNumber,
				FaxNumber = model.FaxNumber,
				
                // Properties not in model
				Subdomain = orginal?.Subdomain,
				CreatedUtc = orginal?.CreatedUtc
			};
		}
	}
}
