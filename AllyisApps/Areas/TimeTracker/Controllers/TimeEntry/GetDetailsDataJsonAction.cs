//------------------------------------------------------------------------------
// <copyright file="GetDetailsDataJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Action to retreive the number of hours spent in a project.
		/// </summary>
		/// <param name="userId">The user's Id.</param>
		/// <param name="startingDate">The starting date of the date range.</param>
		/// <param name="endingDate">The ending date of the date range.</param>
		/// <returns>Json object representing all related projects and their current hours.</returns>
		[HttpPost]
		public ActionResult GetDetailsDataJsonOLD(int userId, DateTime startingDate, DateTime endingDate)
		{
			bool manager = Service.Can(Actions.CoreAction.TimeTrackerEditOthers);

			// Permissiosn checking
			if (userId == Convert.ToInt32(UserContext.UserId))
			{
				if (!Service.Can(Actions.CoreAction.TimeTrackerEditSelf))
				{
					return this.Json(new
					{
						status = "error",
						message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZData
					});
				}
			}
			else
			{
				if (!manager)
				{
					return this.Json(new
					{
						status = "error",
						message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZDataOtherUser
					});
				}
			}

			// Authorized for viewing details
			TimeEntryOverDateRangeViewModel model = this.ConstructTimeEntryOverDataRangeViewModel(
				userId,
				manager,
				TimeTrackerService.GetDayFromDateTime(startingDate),
				TimeTrackerService.GetDayFromDateTime(endingDate)//,
				/*TimeTrackerService.GetLockDate()*/);
			if (model.Projects.Count() == 0 || model.Projects == null)
			{
				return this.Json(new
				{
					status = "error",
					message = Resources.TimeTracker.Controllers.TimeEntry.Strings.WasAnError
				});
			}

			IList<object> result = new List<object>();
			foreach (ProjectHours element in model.ProjectHours)
			{
				result.Add(new { projectName = element.Project.ProjectName, hours = element.GetHoursInHoursMinutes() });
			}

			result.Add(new { projectName = model.GrandTotal.Project.ProjectName, hours = model.GrandTotal.GetHoursInHoursMinutes() });

			return this.Json(new
			{
				status = "success",
				projects = result.ToArray()
			});
		}
	}
}
