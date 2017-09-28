﻿//------------------------------------------------------------------------------
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
using AllyisApps.ViewModels.Staffing;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Staffing controller.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Page for list of applicants.
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <returns></returns>
		public ActionResult ApplicantList(int subscriptionId)
		{
			var subInfo = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId];
			List<Applicant> applicants = this.AppService.GetApplicantsByOrgId(subInfo.OrganizationId);
			ApplicantListViewModel model = new ApplicantListViewModel()
			{
				Applicants = applicants.Select(a => InitializeStaffingApplicantViewModel(a)).ToList()
			};

			return this.View(model);
		}
	}
}