using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Tags View model.
	/// </summary>
	public class ApplicationInfoViewModel
	{
		/// <summary>
		/// name of applicant whom this application belongs
		/// </summary>
		public string ApplicantName { get; set; }

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
		public ApplicationDocumentViewModel ApplicationDocuments { get; set; }
	}
}