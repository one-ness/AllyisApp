//------------------------------------------------------------------------------
// <copyright file="IndexAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;
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
		/// Applicant page.
		/// </summary>
		/// <returns></returns>
		public async Task<ActionResult> Applicant(int subscriptionId, int applicantId)
		{
			SetNavData(subscriptionId);

			Applicant applicant = await this.AppService.GetApplicantAddressById(applicantId);
			StaffingApplicantViewModel model = InitializeStaffingApplicantViewModel(applicant);
			var applicationsGet = await this.AppService.GetApplicationsByApplicantId(applicantId);
			model.Applications = applicationsGet.Select(a => InitializeStaffingApplicationViewModel(a)).ToList();

			foreach (var application in model.Applications)
			{
				application.Applicant = applicant;
			}

			return this.View(model);
		}

		private static StaffingApplicationViewModel InitializeStaffingApplicationViewModel(Application application)
		{
			return new StaffingApplicationViewModel()
			{
				//Applicant = application.Applicant,
				ApplicantId = application.ApplicantId,
				ApplicationCreatedUtc = application.ApplicationCreatedUtc,
				//ApplicationDocuments = application.ApplicationDocuments.Select(d => InitializeApplicationDocumentViewModel(d)).ToList(),
				ApplicationId = application.ApplicationId,
				ApplicationModifiedUtc = application.ApplicationModifiedUtc,
				ApplicationStatus = application.ApplicationStatus,
				Notes = application.Notes,
				PositionId = application.PositionId
			};
		}

		private static ApplicationDocumentViewModel InitializeApplicationDocumentViewModel(ApplicationDocument document)
		{
			return new ApplicationDocumentViewModel()
			{
				ApplicationDocumentId = document.ApplicationDocumentId,
				ApplicationId = document.ApplicationId,
				DocumentLink = document.DocumentLink,
				DocumentName = document.DocumentName
			};
		}

		private static StaffingApplicantViewModel InitializeStaffingApplicantViewModel(Applicant applicant)
		{
			return new StaffingApplicantViewModel()
			{
				Address = applicant.Address,
				AddressId = applicant.AddressId,
				ApplicantId = applicant.ApplicantId,
				City = applicant.City,
				Country = applicant.Country,
				Email = applicant.Email,
				FirstName = applicant.FirstName,
				LastName = applicant.LastName,
				Notes = applicant.Notes,
				PhoneNumber = applicant.PhoneNumber,
				PostalCode = applicant.PostalCode,
				State = applicant.State
			};
		}
	}
}