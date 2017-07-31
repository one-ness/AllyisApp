//------------------------------------------------------------------------------
// <copyright file="EditProfileAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core.Alert;
using AllyisApps.Services;
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
				Address = userInfo.Address,
				City = userInfo.City,
				State = userInfo.State,
				Country = userInfo.Country,
				PostalCode = userInfo.PostalCode,
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
				await Task.Factory.StartNew(() => AppService.SaveUserInfo(new User
				{
					UserId = this.AppService.UserContext.UserId,
					Email = model.Email,
					FirstName = model.FirstName,
					LastName = model.LastName,
					DateOfBirth = AppService.GetDateTimeFromDays(model.DateOfBirth),
					Address = model.Address,
					City = model.City,
					State = model.State,
					Country = model.Country,
					PostalCode = model.PostalCode,
					PhoneNumber = model.PhoneNumber
				}));

				Notifications.Add(new BootstrapAlert(Resources.Strings.UpdateProfileSuccessMessage, Variety.Success));
				this.RouteUserHome();
			}

			model.ValidCountries = AppService.ValidCountries();

			// Invalid Model
			return this.RedirectToAction(ActionConstants.Index, ControllerConstants.Account);
		}
	}
}
