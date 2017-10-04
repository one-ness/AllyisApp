using System.Collections.Generic;
using AllyisApps.Services.StaffingManager;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Staffing Application view model.
	/// </summary>
	public class StaffingApplicationViewModel
	{
		/// <summary>
		/// Gets or sets the application's ID.
		/// </summary>
		[ScaffoldColumn(false)]
		public int ApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the applicant's ID.
		/// </summary>
		[ScaffoldColumn(false)]
		public int ApplicantId { get; set; }

		/// <summary>
		/// Gets or sets the position's ID.
		/// </summary>
		[ScaffoldColumn(false)]
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets the possible positions.
		/// </summary>
		public List<SelectListItem> PositionList { get; set; }

		/// <summary>
		/// Gets or sets the application status ID.
		/// </summary>
		[ScaffoldColumn(false)]
		public int ApplicationStatus { get; set; }

		/// <summary>
		/// Gets DateCreated.
		/// </summary>
		[ScaffoldColumn(false)]
		public DateTime ApplicationCreatedUtc { get; set; }

		/// <summary>
		/// Gets date modified.
		/// </summary>
		[ScaffoldColumn(false)]
		public DateTime ApplicationModifiedUtc { get; set; }

		/// <summary>
		/// Gets or sets Notes.
		/// </summary>
		public string Notes { get; set; }

		/// <summary>
		/// Gets the ApplicationDocument list.
		/// </summary>
		public List<ApplicationDocumentViewModel> ApplicationDocuments { get; set; }

		/// <summary>
		/// Gets the ApplicantDBEntity.
		/// </summary>
		public Applicant Applicant { get; set; }
	}
}