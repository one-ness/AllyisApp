using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Tags View model.
	/// </summary>
	public class ApplicantInfoViewModel
	{
		/// <summary>
		/// name of applicant whom this application belongs
		/// </summary>
		public string ApplicantName { get; set; }

		/// <summary>
		/// applicant first name
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// applicants last name
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// the email given by the applicant
		/// </summary>
		public string ApplicantEmail { get; set; }

		/// <summary>
		/// the phone number given by the applicant
		/// </summary>
		public string ApplicantPhone { get; set; }

		/// <summary>
		/// the phone number given by the applicant
		/// </summary>
		public int ApplicationStatus { get; set; }

		/// <summary>
		/// notes about this application
		/// </summary>
		public string Notes { get; set; }
		
		/// <summary>
		/// Applicants Address
		/// </summary>
		public AddressViewModel ApplicantAddress { get; set; }

		/// <summary>
		/// last time application was edited
		/// </summary>
		public DateTime ApplicationModifiedUtc { get; set; }

	}
}