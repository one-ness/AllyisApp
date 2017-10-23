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
using AllyisApps.ViewModels.Staffing;
using System;
using System.Threading.Tasks;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Staffing controller.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Applicant page.
		/// </summary>
		/// <returns></returns>
		async public Task<ActionResult> DeleteApplication(int applicationId)
		{
			var applicantGet = await this.AppService.GetApplicantAddressByApplicationId(applicationId);
			int applicantId = applicantGet.ApplicantId;
			this.AppService.DeleteApplication(applicationId);
			await Task.Yield();
			return this.RedirectToAction("Applicant", new { applicantId = applicantId });
		}
	}
}