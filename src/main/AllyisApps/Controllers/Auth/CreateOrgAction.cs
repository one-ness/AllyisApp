//------------------------------------------------------------------------------
// <copyright file="CreateOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.Account;
using AllyisApps.Services.Org;
using AllyisApps.ViewModels;

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
			return this.View(new EditOrganizationViewModel() { ValidCountries = AccountService.ValidCountries() });
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
			if (!this.IsSubdomainNameUnique(model.SubdomainName))
			{
				ModelState.AddModelError(string.Empty, Resources.Controllers.Auth.Strings.SubdomainTaken);
			}

			if (model != null && ModelState.IsValid)
			{
				int orgId = OrgService.CreateOrganization(
					new OrganizationInfo()
					{
						Address = model.Address,
						City = model.City,
						Country = model.Country,
						Name = model.Name,
						SiteUrl = model.SiteUrl,
						State = model.State,
						PostalCode = model.PostalCode,
						PhoneNumber = model.PhoneNumber,
						FaxNumber = model.FaxNumber,
						Subdomain = model.SubdomainName
					},
					UserContext.UserId);

				Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.OrganizationCreatedNotification, Variety.Success));

				OrgService.UpdateActiveOrganization(UserContext.UserId, orgId);

				return this.RedirectToSubDomainAction(orgId, null, ActionConstants.OrgIndex);
			}

			// Something happened, reload this view
			return this.View(model);
		}
	}
}