﻿//------------------------------------------------------------------------------
// <copyright file="ManageOrgAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Common.Types;
using AllyisApps.ViewModels.Auth;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Get: Account/Manage/id
		/// The management page for an organization, displays billing, subscriptions, etc.
		/// </summary>
		public ActionResult ManageOrg2(int id)
		{
			var model = new OrganizationDetailsViewModel();
			var org = this.AppService.GetOrganization(id);
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
			return View(model);
		}
	}
}
