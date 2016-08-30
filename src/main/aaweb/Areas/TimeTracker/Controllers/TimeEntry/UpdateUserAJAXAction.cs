//------------------------------------------------------------------------------
// <copyright file="UpdateUserAJAXAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// AJAX callback to update the projects for a user.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <param name="offUser">The list of projects not associated with the user.</param>
		/// <param name="onUser">The list of projects associated with the user.</param>
		/// <returns>Json object representing the results of the action.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult UpdateUserAJAX(int userId, List<int> offUser, List<int> onUser)
		{
			if (AuthorizationService.Can(Services.Account.Actions.CoreAction.EditProject))
			{
				if (offUser != null)
				{
					foreach (int proj_id in offUser)
					{
						if (ProjectService.UpdateProjectUser(proj_id, userId, false).Equals(0))
						{
							ProjectService.DeleteProjectUser(proj_id, userId);
						}
					}
				}

				if (onUser != null)
				{
					foreach (int proj_id in onUser)
					{
						if (ProjectService.UpdateProjectUser(proj_id, userId, true).Equals(0))
						{
							ProjectService.CreateProjectUser(proj_id, userId);
						}
					}
				}

				return this.Json(new { status = "success" });
			}
			else
			{
				return this.Json(new { status = "failure" });
			}
		}
	}
}