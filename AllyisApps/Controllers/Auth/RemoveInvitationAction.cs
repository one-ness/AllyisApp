//------------------------------------------------------------------------------
// <copyright file="RemoveInvitationAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Billing;

namespace AllyisApps.Controllers.Auth
{
	/// <summary>
	/// Controller for account and organization related actions.
	/// </summary>
	public partial class AccountController : BaseController
	{
		/// <summary>
		/// Removes the provided invitation from the invitations table.
		/// </summary>
		/// <param name="checkedIds">Organization's id.</param>
		/// <param name="orgId">organizaitons Id. </param>>
		/// <returns>Redirects to the manage org action.</returns>
		[HttpPost]
		public async Task<ActionResult> RemoveInvitation(string checkedIds, int orgId)
		{
			await AppService.CheckPermission(ProductIdEnum.AllyisApps, AppService.UserAction.Delete, AppService.AppEntity.OrganizationUser, orgId);

			if (checkedIds != "")
			{
				//concat stuff here
				int[] concat = StringToIntList(checkedIds);

				var results = await AppService.RemoveInvitations(concat, orgId);

				if (results)
				{
					Notifications.Add(new BootstrapAlert(Resources.Strings.InvitationDeleteNotification, Variety.Success));
					return RedirectToAction(ActionConstants.OrganizationInvitations, new { id = orgId });
				}
				else
				{
					Notifications.Add(new BootstrapAlert("Deleting Invitation Failed.", Variety.Warning));
					return RedirectToAction(ActionConstants.OrganizationInvitations, new { id = orgId });
				}
			}
			else
			{
				Notifications.Add(new BootstrapAlert("Can't delete 0 invitations", Variety.Warning));
				return RedirectToAction(ActionConstants.OrganizationInvitations, new { id = orgId });
			}
		}

		/// <summary>
		/// seperates out a string list of ints comma seperates, into int array
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public int[] StringToIntList(string str)
		{
			if (String.IsNullOrEmpty(str)) return new int[0];
			var split = str.Split(',');
			var result = new int[split.Length];
			int i = 0;
			foreach (var s in str.Split(','))
			{
				result[i] = Int32.Parse(s);
				i++;
			}
			return result;
		}
	}
}