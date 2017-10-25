//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;

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
		public async Task<ActionResult> DeleteApplication(int applicationId)
		{
			var applicantGet = await AppService.GetApplicantAddressByApplicationId(applicationId);
			int applicantId = applicantGet.ApplicantId;
			AppService.DeleteApplication(applicationId);
			await Task.Yield();
			return RedirectToAction("Applicant", new { applicantId = applicantId });
		}
	}
}