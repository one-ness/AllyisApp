//------------------------------------------------------------------------------
// <copyright file="ReviewEntries.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Returns a new view for approving time entries
		/// </summary>
		/// <param name="subscriptionId">The subscription's Id.</param>
		/// <returns>The action result.</returns>
		public ActionResult Review(int subscriptionId)
		{
			var model = new ReviewViewModel
			{
				UserId = AppService.UserContext.UserId,
				SubscriptionId = subscriptionId,
				SubscriptionName = AppService.GetSubscription(subscriptionId).SubscriptionName,
				PayClasses = AppService.GetPayClassesBySubscriptionId(subscriptionId).Select(x => new PayClassInfoViewModel(x))
			};
			return View(model);
		}
	}
}