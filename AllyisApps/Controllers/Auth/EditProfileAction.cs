//------------------------------------------------------------------------------
// <copyright file="EditProfileAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Lib;
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
		/// GET: /Account/EditProfile.
		/// </summary>
		/// <returns>Edit profile view.</returns>
		public async Task<ActionResult> EditProfile()
		{
			var model = new EditProfileViewModel();
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(AppService);
			var user = await AppService.GetCurrentUserAsync();
			model.Address = user.Address?.Address1;
			model.AddressId = user.Address?.AddressId;
			model.City = user.Address?.City;
			model.DateOfBirth = Utility.GetDaysFromDateTime(user.DateOfBirth);
			model.Email = user.Email;
			model.FirstName = user.FirstName;
			model.LastName = user.LastName;
			model.PhoneNumber = user.PhoneNumber;
			model.PostalCode = user.Address?.PostalCode;
			model.SelectedCountryCode = user.Address?.CountryCode;
			model.SelectedStateId = user.Address?.StateId;
			model.LocalizedStates = ModelHelper.GetLocalizedStates(AppService, model.SelectedCountryCode);

			return View(model);
		}

		/// <summary>
		/// POST: /Account/EditProfile.
		/// </summary>
		/// <param name="model">The EditProfileViewModel.</param>
		/// <returns>Tje Edit profile view.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditProfile(EditProfileViewModel model)
		{
			if (ModelState.IsValid)
			{
				await AppService.UpdateCurrentUserProfile(model.DateOfBirth, model.FirstName, model.LastName, model.PhoneNumber, model.AddressId, model.Address, model.City, model.SelectedStateId, model.PostalCode, model.SelectedCountryCode);
				Notifications.Add(new BootstrapAlert(Resources.Strings.UpdateProfileSuccessMessage, Variety.Success));
				return RouteUserHome();
			}

			// model error
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(AppService);
			model.LocalizedStates = ModelHelper.GetLocalizedStates(AppService, model.SelectedCountryCode);

			return View(model);
		}
	}
}