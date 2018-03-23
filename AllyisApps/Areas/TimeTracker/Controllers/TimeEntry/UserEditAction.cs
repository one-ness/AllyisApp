//------------------------------------------------------------------------------
// <copyright file="UserEditAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Class which manages Time Entry objects.
    /// </summary>
    public partial class TimeEntryController : BaseController
    {
        /// <summary>
        /// GET /TimeTracker/TimeEntry/UserEdit.
        /// </summary>
        /// <param name="subscriptionId">The subscription's id.</param>
        /// <param name="userId">The Id of the user to edit.</param>
        /// <returns>The user edit page.</returns>
        public async Task<ActionResult> UserEdit(int subscriptionId, int userId)
        {
            AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
            var infosTask = AppService.GetProjectsForOrgAndUser(userId, subscriptionId);
            var subscriptionNameToDisplayTask = AppService.GetSubscriptionName(subscriptionId);

            await Task.WhenAll(infosTask, subscriptionNameToDisplayTask);

            var infos = infosTask.Result;
            string subscriptionNameToDisplay = subscriptionNameToDisplayTask.Result;

            return View(new UserEditViewModel
            {
                UserId = AppService.UserContext.UserId,
                SubscriptionId = subscriptionId,
                UserProjects = infos.Item1.AsParallel().Select(proj => new UserEditViewModel.ProjectInfoViewModel
                {
                    ProjectId = proj.ProjectId,
                    ProjectName = proj.ProjectName,
                    CustomerName = proj.OwningCustomer.CustomerName
                }),
                AllProjects = infos.Item2.AsParallel().Select(proj => new UserEditViewModel.ProjectInfoViewModel
                {
                    ProjectName = proj.ProjectName,
                    ProjectId = proj.ProjectId,
                    CustomerName = proj.OwningCustomer.CustomerName
                }),
                UserName = infos.Item3,
                SubscriptionName = subscriptionNameToDisplay
            });
        }

        /// <summary>
        /// AJAX callback to update the projects for a user.
        /// </summary>
        /// <param name="userId">The Id of the user to edit.</param>
        /// <param name="subscriptionId">The subscription's Id.</param>
        /// <param name="offUser">The list of projects not associated with the user.</param>
        /// <param name="onUser">The list of projects associated with the user.</param>
        /// <returns>Json object representing the results of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UserEditAJAX(int userId, int subscriptionId, List<int> offUser, List<int> onUser)
        {
            int organizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
            if (AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId, false))
            {
                if (offUser != null && onUser == null)
                {
                    // Notifications.Add(new BootstrapAlert(Strings.UsersMustHaveAProject, Variety.Danger));
                    return Json(new { status = "failure" });
                }
                if (offUser != null)
                {
                    foreach (int proj_id in offUser)
                    {
                        var project = AppService.GetProject(proj_id);
                        if (onUser == null)
                        {
                            if (project.IsDefault)
                            {
                                continue;
                            }
                        }
                        var update = await AppService.UpdateProjectUser(proj_id, userId, false);
                        if (update.Equals(0))
                        {
                            AppService.DeleteProjectUser(proj_id, userId);
                        }
                    }
                }

                if (onUser != null)
                {
                    foreach (int proj_id in onUser)
                    {
                        var project = AppService.GetProject(proj_id);
                        var update = await AppService.UpdateProjectUser(proj_id, userId, true);
                        if (update.Equals(0))
                        {
                            AppService.CreateProjectUser(proj_id, userId);
                        }
                    }
                }

                return Json(new { status = "success" });
            }
            return Json(new { status = "failure" });
        }
    }
}