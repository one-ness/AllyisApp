//------------------------------------------------------------------------------
// <copyright file="EditProfileAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services.Auth;
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
			User user = await AppService.GetCurrentUser2Async();

			var model = new EditProfileViewModel
			{
				LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService.GetCountries()),
				Address = user.Address?.Address1,
				AddressId = user.Address?.AddressId,
				City = user.Address?.City,
				DateOfBirth = user.DateOfBirth,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				PhoneNumber = user.PhoneNumber,
				PostalCode = user.Address?.PostalCode,
				SelectedCountryCode = user.Address?.CountryCode,
				SelectedStateId = user.Address?.StateId,
				LocalizedStates = ModelHelper.GetLocalizedStates(this.AppService.GetStates(user.Address?.CountryCode))
			};

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
			model.LocalizedCountries = ModelHelper.GetLocalizedCountries(this.AppService.GetCountries());
			model.LocalizedStates = ModelHelper.GetLocalizedStates(this.AppService.GetStates(model.SelectedCountryCode));
			return View(model);
		}
	}
}