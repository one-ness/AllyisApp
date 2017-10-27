﻿//------------------------------------------------------------------------------
// <copyright file="RemoveMemberAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;

namespace AllyisApps.Controllers.Auth
{
    /// <summary>
    /// Controller for account and organization related actions.
    /// </summary>
    public partial class AccountController : BaseController
    {
        /// <summary>
        /// POST: Organization/RemoveUser.
        /// </summary>
        /// <param name="id">Organization Id.</param>
        /// <param name="csvUserIds">User ids.</param>
        /// <returns>Redirects to the manage org action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveMember(int id, string csvUserIds)
        {
            if (string.IsNullOrEmpty(csvUserIds))
            {
                Notifications.Add(new BootstrapAlert("Please select atleast one user to delete", Variety.Danger));
            }

            this.AppService.CheckOrgAction(AppService.OrgAction.EditOrganization, id);

            string[] userIds = csvUserIds.Split(',');
            List<Task> userRemoveTask = new List<Task>();

            foreach (var userIdString in userIds)
            {
                int userId = Convert.ToInt32(userIdString);
                userRemoveTask.Add(AppService.RemoveOrganizationUser(id, userId));
            }

            await Task.WhenAll(userRemoveTask);

            Notifications.Add(new BootstrapAlert(Resources.Strings.UserDeletedSuccessfully, Variety.Success));

            return this.RedirectToAction(ActionConstants.OrganizationMembers, new { id = id });
        }
    }
}