﻿//------------------------------------------------------------------------------
// <copyright file="EditProfileAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.ViewModels;
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
		/// GET: /Account/EditProfile.
		/// </summary>
		public ActionResult EditProfile()
		{
			var model = new EditProfileViewModel();
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService);
			var user = this.AppService.GetCurrentUserInfo();
			model.Address = user.Address?.Address1;
			model.AddressId = user.Address?.AddressId;
			model.City = user.Address?.City;
			model.DateOfBirth = this.AppService.GetDayFromDateTime(user.DateOfBirth);
			model.Email = user.Email;
			model.FirstName = user.FirstName;
			model.LastName = user.LastName;
			model.PhoneNumber = user.PhoneNumber;
			model.PostalCode = user.Address?.PostalCode;
			model.SelectedCountryCode = user.Address?.CountryCode;
			model.SelectedStateId = user.Address?.StateId;
			model.LocalizedStates = ModelHelper.GetLocalizedStates(this.AppService, model.SelectedCountryCode);

			return this.View(model);
		}

		/// <summary>
		/// POST: /Account/EditProfile.
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditProfile(EditProfileViewModel model)
		{
			if (ModelState.IsValid)
			{
				this.AppService.UpdateCurrentUserProfile(model.DateOfBirth, model.FirstName, model.LastName, model.PhoneNumber, model.AddressId, model.Address, model.City, model.SelectedStateId, model.PostalCode, model.SelectedCountryCode);
				Notifications.Add(new BootstrapAlert(Resources.Strings.UpdateProfileSuccessMessage, Variety.Success));
				return this.RouteUserHome();
			}

			// model error
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService);
			model.LocalizedStates = ModelHelper.GetLocalizedStates(this.AppService, model.SelectedCountryCode);
			return View(model);
		}
	}
}
