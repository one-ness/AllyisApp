//------------------------------------------------------------------------------
// <copyright file="EditProfileAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services.BusinessObjects;
using AllyisApps.ViewModels;

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
		/// <param name="returnUrl">The return url to redirect to after form submit.</param>
		/// <returns>The result of this action.</returns>
		public ActionResult EditProfile(string returnUrl)
		{
			UserInfo userInfo = AccountService.GetUserInfo();
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
				DateOfBirth = userInfo.DateOfBirth,
				ValidCountries = AccountService.ValidCountries()
			};

			ViewBag.returnUrl = returnUrl;

			return this.View(model);
		}

		/// <summary>
		/// POST: /Account/EditProfile.
		/// </summary>
		/// <param name="model">The Edit Profile view model.</param>
		/// <param name="returnUrl">The return url, in case the user was redirected here from an application.</param>
		/// <returns>The async task to redirect to the return url, or this page again if the model is invalid.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditProfile(EditProfileViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				await Task.Factory.StartNew(() => AccountService.SaveUserInfo(new UserInfo
				{
					UserId = UserContext.UserId,
					Email = model.Email,
					FirstName = model.FirstName,
					LastName = model.LastName,
					DateOfBirth = model.DateOfBirth,
					Address = model.Address,
					City = model.City,
					State = model.State,
					Country = model.Country,
					PostalCode = model.PostalCode,
					PhoneNumber = model.PhoneNumber
				}));

				Notifications.Add(new BootstrapAlert(Resources.Controllers.Auth.Strings.UpdateProfileSuccessMessage, Variety.Success));

				if (!string.IsNullOrWhiteSpace(returnUrl))
				{
					return this.RedirectToLocal(returnUrl);
				}
			}

			return this.View(model);
		}
	}
}
