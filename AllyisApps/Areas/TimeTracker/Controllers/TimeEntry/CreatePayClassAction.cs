//------------------------------------------------------------------------------
// <copyright file="CreatePayClassAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using System;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Creates a payclass.
		/// </summary>
		/// <param name="newPayClass">The pay class to create.</param>
		/// <param name="subscriptionId">Subscription Id.</param>
		/// <returns>Redirects to the settings view.</returns>
		public ActionResult CreatePayClass(string newPayClass, int subscriptionId)
		{
			if (string.IsNullOrWhiteSpace(newPayClass))
			{
				Notifications.Add(new BootstrapAlert(Resources.Strings.CannotCreateBlankPayClass, Variety.Warning));
			}
			else
			{
				int orgId = AppService.UserContext.UserSubscriptions[subscriptionId].OrganizationId;

				// should put try catch in 'else'. Creating a blank pay class results in Two alerts: "Cannot create blank pay class" and "pay class already exists"
				try
				{
					if (AppService.CreatePayClass(newPayClass, orgId, subscriptionId))
					{
						Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessfulCreatePayClass, Variety.Success));
					}
					else
					{
						// Should only get here on permissions failure
						Notifications.Add(new BootstrapAlert(Resources.Strings.ActionUnauthorizedMessage, Variety.Warning));
					}
				}
				catch (ArgumentException)
				{
					// Pay class already exists
					Notifications.Add(new BootstrapAlert(Resources.Strings.FailureCreatePayClassAlreadyExists, Variety.Danger));
				}
			}

			return this.RedirectToAction(ActionConstants.Settings, new { subscriptionId = subscriptionId, id = this.AppService.UserContext.UserId });
		}
	}
}
