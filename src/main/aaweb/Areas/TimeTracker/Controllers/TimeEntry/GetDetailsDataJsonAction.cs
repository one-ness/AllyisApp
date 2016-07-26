//------------------------------------------------------------------------------
// <copyright file="GetDetailsDataJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Models;
using AllyisApps.Core;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// Action to retreive the number of hours spent in a project.
		/// </summary>
		/// <param name="userId">The user's Id.</param>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="startingDate">The starting date of the date range.</param>
		/// <param name="endingDate">The ending date of the date range.</param>
		/// <returns>Json object representing all related projects and their current hours.</returns>
		[HttpPost]
		public ActionResult GetDetailsDataJson(int userId, int organizationId, DateTime startingDate, DateTime endingDate)
		{
			bool manager = AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers);

			// Permissiosn checking
			if (userId == Convert.ToInt32(UserContext.UserId))
			{
				if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf))
				{
					return this.Json(new
					{
						status = "error",
						message = "You are not authorized to retrieve this data."
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
						message = "You are not authorized to retrive this data for another user!"
					});
				}
			}

			// Authorized for viewing details
			TimeEntryOverDateRangeViewModel model = this.ConstructTimeEntryOverDataRangeViewModel(userId, manager, organizationId, startingDate, endingDate, TimeTrackerService.GetLockDate(organizationId, userId));
			if (model.Projects.Count() == 0 || model.Projects == null)
			{
				return this.Json(new
				{
					status = "error",
					message = "An error has occured. Oops!"
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