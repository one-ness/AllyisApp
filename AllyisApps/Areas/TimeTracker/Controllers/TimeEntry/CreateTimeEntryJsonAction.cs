//------------------------------------------------------------------------------
// <copyright file="CreateTimeEntryJsonAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.TimeTracker;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// Creates a new Time Entry based on the model.
		/// </summary>
		/// <param name="subscriptionId">The subscription that the user belongs to.</param>
		/// <param name="entry">The model representing a time entry.</param>
		/// <returns>A result enum, indicating what failed, or a success.</returns>
		public async Task<CreateUpdateTimeEntryResult> CreateTimeEntry(int subscriptionId, TimeEntry entry)
		{
			if (entry.UserId != AppService.UserContext.UserId && !AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId))
			{
				return CreateUpdateTimeEntryResult.NotAuthZTimeEntryOtherUserEdit;
			}

			// Authorized to edit this entry
			int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;

			CreateUpdateTimeEntryResult validationResult = await AppService.ValidateTimeEntryCreateUpdate(entry, organizationId);

			if (validationResult != CreateUpdateTimeEntryResult.Success)
			{
				return validationResult;
			}

			//validate correct project
			var projects = await AppService.GetProjectsByUserAndOrganization(entry.UserId, organizationId);
			var project = projects.SingleOrDefault(p => entry.ProjectId == p.ProjectId);
			if (project == null || !project.IsUserActive)
			{
				return CreateUpdateTimeEntryResult.MustBeAssignedToProject;
			}

			if (project.StartDate != null && entry.Date < project.StartDate
				|| project.EndDate != null && entry.Date > project.EndDate)
			{
				return CreateUpdateTimeEntryResult.ProjectIsNotActive;
			}

			//Update overtime
			entry.Duration = await AppService.GenerateOvertimeFromTimeEntry(organizationId, entry);

			// if duration is 0 if all the hours went to overtime
			if (entry.Duration > 0)
			{
				await AppService.CreateTimeEntry(entry);
			}

			return CreateUpdateTimeEntryResult.Success;
		}
	}
}