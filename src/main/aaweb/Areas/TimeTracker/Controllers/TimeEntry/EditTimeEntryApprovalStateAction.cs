//------------------------------------------------------------------------------
// <copyright file="EditTimeEntryApprovalStateAction.cs" company="Allyis, Inc.">
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
		/// Edits the approval state of the provided ApprovalDataModel objects.
		/// </summary>
		/// <param name="model">A collection of Approval data objects.</param>
		/// <returns>Any errors as JSON objects, else returns JSON object of status = sucess.</returns>
		public ActionResult EditTimeEntryApprovalState(IEnumerable<ApprovalDataModel> model)
		{
			if (model == null || model.Count() == 0)
			{
				return this.Json(new
				{
					status = "error",
					message = "There are no entries to approve here!",
					e = new UnauthorizedAccessException("There are no entries to approve here!")
				});
			}

			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers))
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
							message = "There was an error.",
							response = "REFRESH"
						});
					}
				}

				if (result.Count != 0)
				{
					return this.Json(new
					{
						status = "error",
						message = "There was an error.",
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
				message = "You are not authorized to approve time entries!",
				e = new UnauthorizedAccessException("You are not authorized to approve time entries!")
			});
		}
	}
}