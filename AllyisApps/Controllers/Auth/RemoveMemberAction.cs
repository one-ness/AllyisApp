//------------------------------------------------------------------------------
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
using System.Linq;
using AllyisApps.Services.Billing;

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

            await this.AppService.CheckPermissionAsync(ProductIdEnum.AllyisApps, AppService.UserAction.Edit, AppService.AppEntity.OrganizationUser, id);

			List<int> userIds = csvUserIds.Split(',').Select(userIdString => Convert.ToInt32(userIdString)).ToList();
			try
			{
				await AppService.DeleteOrganizationUsers(userIds, id);
				Notifications.Add(new BootstrapAlert(Resources.Strings.UserDeletedSuccessfully, Variety.Success));
			}
			catch (ArgumentNullException)
			{
				Notifications.Add(new BootstrapAlert("You must select users to remove from the organization.", Variety.Warning));
			}
			catch (ArgumentException)
			{
				Notifications.Add(new BootstrapAlert("Cannot delete yourself from an organization.", Variety.Danger));
			}

			

            return this.RedirectToAction(ActionConstants.OrganizationMembers, new { id = id });
        }
    }
}