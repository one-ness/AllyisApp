//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services.StaffingManager;
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
		public async Task<ActionResult> ApplicantList(int subscriptionId)
		{
			SetNavData(subscriptionId);

			var subInfo = AppService.UserContext.SubscriptionsAndRoles[subscriptionId];
			List<Applicant> applicants = await AppService.GetApplicantAddressesByOrgId(subInfo.OrganizationId);
			ApplicantListViewModel model = new ApplicantListViewModel()
			{
				Applicants = applicants.Select(a => InitializeStaffingApplicantViewModel(a)).ToList()
			};

			return View(model);
		}
	}
}