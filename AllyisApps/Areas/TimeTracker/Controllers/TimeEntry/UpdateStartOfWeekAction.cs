//------------------------------------------------------------------------------
// <copyright file="UpdateStartOfWeekAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Updates the start of week for an Organization.
		/// </summary>
		/// <param name="subscriptionId">The subscription's id.</param>
		/// <param name="startOfWeek">Start of week selected by Organization admin.</param>
		/// <returns>Action result.</returns>
		public ActionResult UpdateStartOfWeek(int subscriptionId, int startOfWeek)
		{
			System.Diagnostics.Debug.WriteLine("New Start Date: " + startOfWeek);
			if (startOfWeek < 0 || startOfWeek > 6)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.InvalidSOW, Variety.Warning));
			}
			else if (!AppService.UpdateStartOfWeek(AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId, subscriptionId, startOfWeek))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
			}
			else
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulSOW, Variety.Success));
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionid = subscriptionId, id = this.AppService.UserContext.UserId });
		}
	}
}
