//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Areas.StaffingManager.ViewModels.Staffing;
using AllyisApps.Controllers;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Areas.StaffingManager.Controllers
{
	/// <summary>
	/// Staffing controller.
	/// </summary>
	public partial class StaffingController : BaseController
	{
		/// <summary>
		/// Create Applicant Page.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult CreateApplicant(int subscriptionId)
		{
			SetNavData(subscriptionId);

			StaffingApplicantViewModel model = new StaffingApplicantViewModel();
			return View(model);
		}

		/// <summary>
		/// Create Applicant.
		/// </summary>
		/// <param name="subscriptionId"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateApplicant(int subscriptionId, StaffingApplicantViewModel model)
		{
			var subInfo = AppService.UserContext.SubscriptionsAndRoles[subscriptionId];
			Applicant applicant = InitializeApplicant(subInfo.OrganizationId, model);
			await AppService.CreateApplicant(applicant);
			return RedirectToAction("ApplicantList");
		}

		private static Applicant InitializeApplicant(int orgId, StaffingApplicantViewModel model)
		{
			return new Applicant
			{
				Address = model.Address,
				//AddressId = model.AddressId,
				//ApplicantId = model.ApplicantId,
				City = model.City,
				Country = model.Country,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Notes = model.Notes,
				OrgId = orgId,
				PhoneNumber = model.PhoneNumber,
				PostalCode = model.PostalCode,
				State = model.State
			};
		}

		///// <summary>
		///// Applicant page.
		///// </summary>
		///// <returns></returns>
		//public ActionResult Applicant(int applicantId)
		//{
		//	Applicant applicant = this.AppService.GetApplicantById(applicantId);
		//	StaffingApplicantViewModel model = InitializeStaffingApplicantViewModel(applicant);

		//	return this.View(model);
		//}

		//private static StaffingApplicantViewModel InitializeStaffingApplicantViewModel(Applicant applicant)
		//{
		//	return new StaffingApplicantViewModel()
		//	{
		//		Address = applicant.Address,
		//		AddressId = applicant.AddressId,
		//		ApplicantId = applicant.ApplicantId,
		//		City = applicant.City,
		//		Country = applicant.Country,
		//		Email = applicant.Email,
		//		FirstName = applicant.FirstName,
		//		LastName = applicant.LastName,
		//		Notes = applicant.Notes,
		//		PhoneNumber = applicant.PhoneNumber,
		//		PostalCode = applicant.PostalCode,
		//		State = applicant.State
		//	};
		//}
	}
}