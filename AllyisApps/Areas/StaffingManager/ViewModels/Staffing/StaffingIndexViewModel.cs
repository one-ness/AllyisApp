using AllyisApps.Services.StaffingManager;
using System.Collections.Generic;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Staffing home page view model.
	/// </summary>
	public class StaffingIndexViewModel
	{
		/// <summary>
		/// a list of the short-hand position objects used to populate the views main table
		/// </summary>
		public List<PositionThumbnailInfo> positions { get; set; }

		/// <summary>
		/// organizations identifier number
		/// </summary>
		public int organizationId { get; set; }

		/// <summary>
		/// subscriptions identifier number
		/// </summary>
		public int subscriptionId { get; set; }

		/// <summary>
		/// subscription name
		/// </summary>
		public string subscriptionName { get; set; }

		/// <summary>
		/// users identification number
		/// </summary>
		public int userId { get; set; }

		/// <summary>
		/// list fo tag objects used by the organization
		/// </summary>
		public List<Services.Lookup.Tag> tags { get; set; }
		
		/// <summary>
		/// list of employment types used by the organization
		/// </summary>
		public List<EmploymentType> employmentTypes { get; set; }

		/// <summary>
		/// list of position levels used by the organization
		/// </summary>
		public List<PositionLevel> positionLevels { get; set; }

		/// <summary>
		/// list of position statuses used by the organization
		/// </summary>
		public List<PositionStatus> positionStatuses { get; set; }
	}
}