//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Crm;
using AllyisApps.Services.StaffingManager;
using AllyisApps.Services.Lookup;
using System;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Staffing controller.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Create Application Page.
		/// </summary>
		/// <returns>A create application page.</returns>
		[HttpGet]
		public ActionResult ApplicationsPartial(int subscriptionId, List<StaffingApplicationViewModel> model)
		{
			var subInfo = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId];
			List<Position> positions = this.AppService.GetPositionsByOrganizationId(subInfo.OrganizationId);
			List<SelectListItem> positionList = new List<SelectListItem>();
			foreach (Position pos in positions)
			{
				positionList.Add(new SelectListItem()
				{
					Text = pos.PositionTitle,
					Value = pos.PositionId.ToString()
				});
			}

			foreach (var mod in model)
			{
				mod.PositionList = positionList;
			}

			return this.PartialView(model);
		}
	}
}