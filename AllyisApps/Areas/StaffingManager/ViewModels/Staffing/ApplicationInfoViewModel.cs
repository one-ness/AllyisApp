﻿using System;
using System.Collections.Generic;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Tags View model.
	/// </summary>
	public class ApplicationInfoViewModel
	{
		/// <summary>
		/// id of application
		/// </summary>
		public int ApplicationId { get; set; }

		/// <summary>
		/// name of applicant whom this application belongs
		/// </summary>
		public string ApplicantName { get; set; }

		/// <summary>
		/// email for the applicant
		/// </summary>
		public string ApplicantEmail { get; set; }

		/// <summary>
		/// basic string address for the applicant
		/// </summary>
		public string ApplicantAddress { get; set; } 

		/// <summary>
		/// status enum id for this application
		/// </summary>
		public int AppliationStatusId { get; set; }

		/// <summary>
		/// when this application was last edited
		/// </summary>
		public DateTime ApplicationModifiedUTC { get; set; }

		/// <summary>
		/// notes about this application
		/// </summary>
		public string Notes { get; set; }

		/// <summary>
		/// the documents attached to this application (ie: resumes, CVs, etc)
		/// </summary>
		public List<ApplicationDocumentViewModel> ApplicationDocuments { get; set; }

		/// <summary>
		/// applications information for all applications to the position
		/// </summary>
		public List<ApplicationStatusSelectViewModel> ApplicationStatuses { get; set; }
	}
}