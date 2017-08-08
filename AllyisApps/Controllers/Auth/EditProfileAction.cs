//------------------------------------------------------------------------------
// <copyright file="EditProfileAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Lookup;
using AllyisApps.ViewModels.Auth;

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
			User userInfo = AppService.GetCurrentUser();
			EditProfileViewModel model = new EditProfileViewModel
			{
				Email = userInfo.Email,
				FirstName = userInfo.FirstName,
				LastName = userInfo.LastName,
				AddressId = userInfo.Address.AddressId,
				Address = userInfo.Address.Address1,
				City = userInfo.Address.City,
				SelectedStateId = userInfo.Address.StateId,
				SelectedCountryCode = userInfo.Address.CountryCode,
				PostalCode = userInfo.Address.PostalCode,
				PhoneNumber = userInfo.PhoneNumber,
				DateOfBirth = AppService.GetDayFromDateTime(userInfo.DateOfBirth)
			};

			// create localized countries
			var countries = this.AppService.GetCountries();
			foreach (var item in countries)
			{
				// get the country name
				string countryName = Utility.AggregateSpaces(item.Value);

				// use the country name in the resource file to get it's localized name
				string localized = Resources.Countries.ResourceManager.GetString(countryName) ?? item.Value;

				// add it to localized countries dictionary
				model.LocalizedCountries.Add(item.Key, localized);
			}

			return this.View(model);
		}

		/// <summary>
		/// POST: /Account/EditProfile.
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditProfile(EditProfileViewModel model)
		{
			if (ModelState.IsValid)
			{
				User user = new Services.User
				{
					UserId = this.AppService.UserContext.UserId,
					Email = model.Email,
					FirstName = model.FirstName,
					LastName = model.LastName,
					DateOfBirth = AppService.GetDateTimeFromDays(model.DateOfBirth),
					PhoneNumber = model.PhoneNumber
				};
				user.Address = new Address
				{
					AddressId = model.AddressId,
					Address1 = model.Address,
					City = model.City,
					StateId = model.SelectedStateId,
					CountryCode = model.SelectedCountryCode,
					PostalCode = model.PostalCode
				};
				await Task.Factory.StartNew(() => AppService.SaveUserInfo(user));

				Notifications.Add(new BootstrapAlert(Resources.Strings.UpdateProfileSuccessMessage, Variety.Success));
				return this.RouteUserHome();
			}

			//model.ValidCountries = AppService.GetCountries();

			// Invalid Model
			return this.View(model);
		}
	}
}
