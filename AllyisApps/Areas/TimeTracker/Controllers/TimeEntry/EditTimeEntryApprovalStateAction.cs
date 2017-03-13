﻿//------------------------------------------------------------------------------
// <copyright file="EditTimeEntryApprovalStateAction.cs" company="Allyis, Inc.">
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
		/// Edits the approval state of the provided ApprovalDataModel objects.
		/// </summary>
		/// <param name="model">A collection of Approval data objects.</param>
		/// <returns>Any errors as JSON objects, else returns JSON object of status = sucess.</returns>
		public ActionResult EditTimeEntryApprovalStateOLD(IEnumerable<ApprovalDataModel> model)
		{
			if (model == null || model.Count() == 0)
			{
				return this.Json(new
				{
					status = "error",
					message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NoEntriesToApprove,
					e = new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.NoEntriesToApprove)
				});
			}

			if (Service.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				IList<object> result = new List<object>();
				foreach (ApprovalDataModel data in model)
				{
					try
					{
						TimeTrackerService.SetTimeEntryApprovalStateById(data.TimeEntryId, data.ApprovalState);
					}
					catch
					{
						result.Add(new
						{
							id = data.TimeEntryId,
							status = "error",
							message = Resources.TimeTracker.Controllers.TimeEntry.Strings.WasAnError,
							response = "REFRESH"
						});
					}
				}

				if (result.Count != 0)
				{
					return this.Json(new
					{
						status = "error",
						message = Resources.TimeTracker.Controllers.TimeEntry.Strings.WasAnError,
						action = "REFRESH",
						errors = result.ToArray()
					});
				}

				return this.Json(new { status = "success" });
			}

			// Permissions failure
			return this.Json(new
			{
				status = "error",
				message = Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZApproveTimeEntries,
				e = new UnauthorizedAccessException(Resources.TimeTracker.Controllers.TimeEntry.Strings.NotAuthZApproveTimeEntries)
			});
		}
	}
}
