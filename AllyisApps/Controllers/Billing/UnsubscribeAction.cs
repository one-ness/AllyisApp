//------------------------------------------------------------------------------
// <copyright file="UnsubscribeAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Removes the selected subscription from the database.
		/// </summary>
		[HttpGet]
		public async Task<ActionResult> Unsubscribe(int id)
		{
			int orgId = await AppService.DeleteSubscription(id);
			Notifications.Add(new BootstrapAlert("Your subscription was deleted successfully.", Variety.Success));
			return RedirectToAction(ActionConstants.OrganizationSubscriptions, new { id = orgId });
		}
	}
}
