using System.Collections.Generic;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Staffing home page view model.
	/// </summary>
	public class StaffingIndexViewModel
	{
		/// <summary>
		/// Gets or sets a list of the short-hand position objects used to populate the views main table.
		/// </summary>
		public List<PositionThumbnailInfo> Positions { get; set; }

		/// <summary>
		/// Gets or sets organizations identifier number.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets subscriptions identifier number.
		/// </summary>
		public int SubscriptionId { get; set; }

		/// <summary>
		/// Gets or sets subscription name.
		/// </summary>
		public string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets users identification number.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets list fo tag objects used by the organization.
		/// </summary>
		public List<Services.Lookup.Tag> Tags { get; set; }

		/// <summary>
		/// Gets or sets list of employment types used by the organization.
		/// </summary>
		public List<EmploymentType> EmploymentTypes { get; set; }

		/// <summary>
		/// Gets or sets list of position levels used by the organization.
		/// </summary>
		public List<PositionLevel> PositionLevels { get; set; }

		/// <summary>
		/// Gets or sets list of position statuses used by the organization.
		/// </summary>
		public List<PositionStatus> PositionStatuses { get; set; }
	}
}