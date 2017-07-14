//------------------------------------------------------------------------------
// <copyright file="EditMemberAction.cs" company="Allyis, Inc.">
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
	/// Controller for editing organization member attributes.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// POST: /Account/EditMember.
		/// </summary>
		/// <param name="id">Id of the org member to edit</param>
		/// <returns>The async task to redirect to the return url, or this page again if the model is invalid.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditMember(int id)
		{
			User userInfo = AppService.GetUser(id);
			return this.View(userInfo);
		}
	}
}