//------------------------------------------------------------------------------
// <copyright file="EditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels;

namespace AllyisApps.Controllers
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// POST: Account/Edit.
		/// </summary>
		/// <param name="model">The organization ViewModel POST data.</param>
		/// <returns>Manage Page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(EditOrganizationViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (Service.UpdateOrganization(
					new OrganizationInfo()
					{
						OrganizationId = UserContext.ChosenOrganizationId,
						Name = model.Name,
						SiteUrl = model.SiteUrl,
						Address = model.Address,
						City = model.City,
						State = model.State,
						Country = model.Country,
						PostalCode = model.PostalCode,
						PhoneNumber = model.PhoneNumber,
						FaxNumber = model.FaxNumber,
					}))
				{
					// Organization updated successfully
					Notifications.Add(new BootstrapAlert(@Resources.Controllers.Auth.Strings.OrganizationDetailsUpdated, Variety.Success));
					return this.RedirectToAction(ActionConstants.Manage);
				}

				// Organization update failed due to invalid permissions
				return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Controllers.Auth.Strings.CannotEditProfileMessage), ControllerConstants.Organization, ActionConstants.Edit));
			}

			// Model is invalid, try again
			return this.View(model);
		}

		/// <summary>
		/// GET: /Account/Edit.
		/// The page for editing an organization's information.
		/// </summary>
		/// <param name="returnUrl">The return url to redirect to after form submit.</param>
		/// <returns>The result of this action.</returns>
		public ActionResult Edit(string returnUrl)
		{
			if (Service.Can(Actions.CoreAction.EditOrganization))
			{
				EditOrganizationViewModel model = this.ConstructEditOrganizationViewModel(
					Service.GetOrganization(UserContext.ChosenOrganizationId),
					Service.Can(Actions.CoreAction.EditOrganization),
					Service.ValidCountries());

				ViewBag.returnUrl = returnUrl;

				return this.View(model);
			}

			ViewBag.ErrorInfo = "Permission";
			return this.View(ViewConstants.Error, new HandleErrorInfo(new UnauthorizedAccessException(@Resources.Controllers.Auth.Strings.CannotEditProfileMessage), ControllerConstants.Organization, ActionConstants.Edit));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EditOrganizationViewModel"/> class.
		/// </summary>
		/// <param name="organization">An <see cref="OrganizationInfo"/> for the organization.</param>
		/// <param name="canDelete">The users permission to delete.</param>
		/// <param name="validCountries">List of valid countries.</param>
		/// <returns>An initialized EditOrganizationViewModel.</returns>
		public EditOrganizationViewModel ConstructEditOrganizationViewModel(OrganizationInfo organization, bool canDelete, IEnumerable<string> validCountries)
		{
			return new EditOrganizationViewModel
			{
				OrganizationId = organization.OrganizationId,
				Name = organization.Name,
				SiteUrl = organization.SiteUrl,
				Address = organization.Address,
				City = organization.City,
				State = organization.State,
				Country = organization.Country,
				PostalCode = organization.PostalCode,
				PhoneNumber = organization.PhoneNumber,
				FaxNumber = organization.FaxNumber,
				SubdomainName = organization.Subdomain,
				CanDelete = canDelete,
				ValidCountries = validCountries
			};
		}
	}
}