//------------------------------------------------------------------------------
// <copyright file="OrgDetailsAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Services;
using AllyisApps.ViewModels.Auth;
using AllyisApps.Services.Billing;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get: Account/OrgDetails
		/// </summary>
		public async Task<ActionResult> OrgDetails(int id)
		{
			var model = new OrganizationDetailsViewModel();
			model.CanEditOrganization = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Edit, AppService.AppEntity.Organization, id, false);
			var org = await AppService.GetOrganizationAsync(id);
			model.CanDeleteOrganization = await AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Delete, AppService.AppEntity.Organization, id, false);
			if (org.Address != null)
			{
				model.Address = org.Address.Address1;
				model.City = org.Address.City;
				model.CountryName = org.Address.CountryName;
				model.PostalCode = org.Address.PostalCode;
				model.StateName = org.Address.StateName;
			}

			model.FaxNumber = org.FaxNumber;
			model.OrganizationId = org.OrganizationId;
			model.OrganizationName = org.OrganizationName;
			model.PhoneNumber = org.PhoneNumber;
			model.SiteURL = org.SiteUrl;
			model.Subdomain = org.Subdomain;

			return View(model);
		}
	}
}