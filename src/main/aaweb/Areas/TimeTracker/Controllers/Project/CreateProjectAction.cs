//------------------------------------------------------------------------------
// <copyright file="CreateProjectAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.ViewModels;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseProductController
	{
		/// <summary>
		/// Creates a new project using a <see cref="EditProjectViewModel"/>.
		/// </summary>
		/// <param name="model"><see cref="EditProjectViewModel"/> representing new project.</param>
		/// <returns>The Project ID.</returns>
		public int CreateProject(EditProjectViewModel model)
		{
			if (model.OrganizationId == null)
			{
				return ProjectService.CreateProjectFromCustomerIdOnly(
					model.ParentCustomerId, 
					model.ProjectName, 
					model.PriceType, 
					TimeTrackerService.GetDateTimeFromDays(model.StartDate), 
					TimeTrackerService.GetDateTimeFromDays(model.EndDate));
			}
			else
			{
				return ProjectService.CreateProject(
					(int)model.OrganizationId, 
					model.ParentCustomerId, 
					model.ProjectName, 
					model.PriceType, 
					TimeTrackerService.GetDateTimeFromDays(model.StartDate), 
					TimeTrackerService.GetDateTimeFromDays(model.EndDate));
			}
		}
	}
}