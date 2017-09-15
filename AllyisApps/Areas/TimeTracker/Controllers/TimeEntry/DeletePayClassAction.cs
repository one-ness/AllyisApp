//------------------------------------------------------------------------------
// <copyright file="DeletePayClassAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
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
		/// Deletes a payclass from an org.
		/// </summary>
		/// <param name="userId">The id of the class to delete.</param> // TODO: update this after changing the route
		/// <param name="subscriptionId">The subscription's id.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult DeletePayClass(int userId, int subscriptionId)
		{
			int orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

			string sourcePayClassName = AppService.GetPayClasses(subscriptionId).First(pc => pc.PayClassId == userId).PayClassName;

			// Built-in, non-editable pay classes cannot be deleted
			// Used pay classes cannot be deleted, suggest manager to merge it with another payclass instead
			if (sourcePayClassName == "Regular" || sourcePayClassName == "Overtime" || sourcePayClassName == "Holiday" || sourcePayClassName == "Paid Time Off" || sourcePayClassName == "Unpaid Time Off" || AppService.GetTimeEntriesThatUseAPayClass(userId).Count() > 0)
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotDeletePayClass, Variety.Warning));
			}
			else
			{
				if (AppService.DeletePayClass(userId, orgId, subscriptionId, null))
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulDeletePayClass.Replace("{0}", sourcePayClassName), Variety.Success));
				}
				else
				{
					// Should only be here because of permission failures
					Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
				}
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId });
		}
	}
}