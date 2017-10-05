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
using System;
using AllyisApps.Lib;

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

			return this.PartialView(model);
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
			application.ApplicationId = this.AppService.CreateApplication(application);
			UploadAttachments(model, application);
			return this.RedirectToAction("Applicant", new { applicantId = model.ApplicantId });
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
		
		private void UploadAttachments(StaffingApplicationViewModel model, Application application)
		{
			foreach (string name in AzureFiles.GetReportAttachments(application.ApplicationId))
			{
				if (model.ApplicationDocuments == null || !model.ApplicationDocuments.Select(d => d.DocumentName).Contains(name))
				{
					AzureFiles.DeleteApplicationDocument(application.ApplicationId, name);
				}
			}

			if (model.ApplicationDocuments != null)
			{
				foreach (ApplicationDocumentViewModel document in model.ApplicationDocuments)
				{
					if (document != null)
					{
						AzureFiles.SaveReportAttachments(application.ApplicationId, document.InputStream, document.DocumentName);
					}
				}
			}
		}
	}
}