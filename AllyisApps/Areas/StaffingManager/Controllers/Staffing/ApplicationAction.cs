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
		public ActionResult Application(int subscriptionId, int applicantId)
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

			StaffingApplicationViewModel model = new StaffingApplicationViewModel()
			{
				ApplicantId = applicantId,
				PositionList = positionList
			};

			return this.View(model);
		}

		/// <summary>
		/// Create Application.
		/// </summary>
		/// <param name="model"></param>
		/// <returns>A redirect to staffing index page.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Application(StaffingApplicationViewModel model)
		{
			Application application = InitializeApplication(model);
			int applicationId = this.AppService.CreateApplication(application);
			return this.RedirectToAction("Index");
		}

		private static Application InitializeApplication(StaffingApplicationViewModel model)
		{
			List<ApplicationDocument> documents = model.ApplicationDocuments != null ?
				model.ApplicationDocuments.Select(d => InitializeApplicationDocument(d)).ToList() :
				new List<ApplicationDocument>();

			return new Application()
			{
				Applicant = model.Applicant,
				ApplicantId = model.ApplicantId,
				ApplicationCreatedUtc = model.ApplicationCreatedUtc,
				ApplicationDocuments = documents,
				ApplicationId = 1, // this will be replaced by stored procedure
				ApplicationModifiedUtc = model.ApplicationModifiedUtc,
				ApplicationStatus = model.ApplicationStatus,
				Notes = model.Notes,
				PositionId = model.PositionId
			};
		}

		private static ApplicationDocument InitializeApplicationDocument(ApplicationDocumentViewModel d)
		{
			return new ApplicationDocument()
			{
				ApplicationDocumentId = d.ApplicationDocumentId,
				ApplicationId = d.ApplicationId,
				DocumentLink = d.DocumentLink,
				DocumentName = d.DocumentName
			};
		}

		///// <summary>
		///// Application page.
		///// </summary>
		///// <returns></returns>
		//public ActionResult Application(int applicationId)
		//{
		//	Application application = this.AppService.GetApplicationById(applicationId);
		//	StaffingApplicationViewModel model = InitializeStaffingApplicationViewModel(application);

		//	return this.View(model);
		//}

		//private StaffingApplicationViewModel InitializeStaffingApplicationViewModel(Application application)
		//{
		//	return new StaffingApplicationViewModel()
		//	{
		//		ApplicantId = application.ApplicantId,
		//		ApplicationCreatedUtc = application.ApplicationCreatedUtc,
		//		ApplicationDocuments = application.ApplicationDocuments.Select(d => InitializeApplicationDocumentViewModel(d)).ToList(),
		//		ApplicationId = application.ApplicationId,
		//		ApplicationModifiedUtc = application.ApplicationModifiedUtc,
		//		ApplicationStatus = application.ApplicationStatus,
		//		Notes = application.Notes,
		//		PositionId = application.PositionId
		//	};
		//}

		//private ApplicationDocumentViewModel InitializeApplicationDocumentViewModel(ApplicationDocument document)
		//{
		//	return new ApplicationDocumentViewModel()
		//	{
		//		ApplicationDocumentId = document.ApplicationDocumentId,
		//		ApplicationId = document.ApplicationId,
		//		DocumentLink = document.DocumentLink,
		//		DocumentName = document.DocumentName
		//	};
		//}
	}
}