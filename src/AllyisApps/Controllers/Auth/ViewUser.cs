//------------------------------------------------------------------------------
// <copyright file="EditProfileAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
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
		/// <param name="id">The user id.</param>
		/// <returns>The result of this action.</returns>
		public ActionResult ViewUser(int id)
		{
			UserInfo userInfo = Service.GetUserInfo(id);
			EditProfileViewModel model = new EditProfileViewModel(false)
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
				ValidCountries = Service.ValidCountries()
			};

			return this.View(model);
		}
	}
}
