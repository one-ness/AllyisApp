//------------------------------------------------------------------------------
// <copyright file="EditProfileAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Lookup;
using AllyisApps.ViewModels.Auth;
using System.Threading.Tasks;
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
			User userInfo = AppService.GetCurrentUser();
			EditProfileViewModel model = new EditProfileViewModel
			{
				Email = userInfo.Email,
				FirstName = userInfo.FirstName,
				LastName = userInfo.LastName,
				AddressId = userInfo.AddressId,
				Address = userInfo.Address.Address1,
				City = userInfo.Address.City,
				State = userInfo.Address.State,
				Country = userInfo.Address.CountryId,
				PostalCode = userInfo.Address.PostalCode,
				PhoneNumber = userInfo.PhoneNumber,
				DateOfBirth = AppService.GetDayFromDateTime(userInfo.DateOfBirth),
				ValidCountries = AppService.ValidCountries()
			};

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
					AddressId = model.AddressId,
					PhoneNumber = model.PhoneNumber
				};
				user.Address = new Address
				{
					Address1 = model.Address,
					City = model.City,
					State = model.State,
					CountryId = model.Country,
					PostalCode = model.PostalCode
				};
				await Task.Factory.StartNew(() => AppService.SaveUserInfo(user));

				Notifications.Add(new BootstrapAlert(Resources.Strings.UpdateProfileSuccessMessage, Variety.Success));
				this.RouteUserHome();
			}

			model.ValidCountries = AppService.ValidCountries();

			// Invalid Model
			return this.View(model);
		}
	}
}
