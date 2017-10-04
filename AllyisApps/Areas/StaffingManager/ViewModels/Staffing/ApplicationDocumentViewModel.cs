using System.Collections.Generic;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Staffing application document view model.
	/// </summary>
	public class ApplicationDocumentViewModel
	{
		/// <summary>
		/// Gets or sets the index of the file.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Gets or sets the ApplicationDocumentId.
		/// </summary>
		public int ApplicationDocumentId { get; set; }

		/// <summary>
		/// Gets or sets the application's ID.
		/// </summary>
		public int ApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the DocumentLink.
		/// </summary>
		public string DocumentLink { get; set; }

		/// <summary>
		/// Gets or sets the DocumentName.
		/// </summary>
		public string DocumentName { get; set; }
	}
}