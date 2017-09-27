using System.Collections.Generic;
using AllyisApps.Services.StaffingManager;
using System;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Staffing Application view model.
	/// </summary>
	public class StaffingApplicationViewModel
	{       /// <summary>
			/// Gets or sets the application's ID.
			/// </summary>
		public int ApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the applicant's ID.
		/// </summary>
		public int ApplicantId { get; set; }

		/// <summary>
		/// Gets or sets the position's ID.
		/// </summary>
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets the application status ID.
		/// </summary>
		public ApplicationStatusEnum ApplicationStatus { get; set; }

		/// <summary>
		/// Gets DateCreated.
		/// </summary>
		public DateTime ApplicationCreatedUtc { get; set; }

		/// <summary>
		/// Gets date modified.
		/// </summary>
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