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
		public ActionResult Application()
		{
			StaffingApplicationViewModel model = new StaffingApplicationViewModel();
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
			this.AppService.CreateApplication(application);
			return this.RedirectToAction("Index");
		}

		private static Application InitializeApplication(StaffingApplicationViewModel model)
		{
			return new Application()
			{
				Applicant = model.Applicant,
				ApplicantId = model.ApplicantId,
				ApplicationCreatedUtc = model.ApplicationCreatedUtc,
				ApplicationDocuments = model.ApplicationDocuments.Select(d => InitializeApplicationDocument(d)).ToList(),
				ApplicationId = model.ApplicationId,
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