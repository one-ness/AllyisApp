using System;
using System.Collections.Generic;
using AllyisApps.Services.Lookup;

namespace AllyisApps.Services.StaffingManager
{
	public class PositionThumbnailInfo
	{
		/// <summary>
		/// Gets or sets the position's Id.
		/// </summary>
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets positions associated organization.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets positionss hiring customer.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets positionss hiring customer name.
		/// </summary>
		public string CustomerName { get; set; }

		/// <summary>
		/// Gets when the position was last modified.
		/// </summary>
		public DateTime PositionModifiedUtc { get; set; }

		/// <summary>
		/// Get or sets the Start date of the position.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the position name.
		/// </summary>
		public string PositionTitle { get; set; }

		/// <summary>
		/// Gets or sets the number of hires needed for this position.
		/// </summary>
		public int PositionCount { get; set; }

		/// <summary>
		/// Gets or sets positionss hiring manager.
		/// </summary>
		public string HiringManager { get; set; }

		/// <summary>
		/// Gets or sets the name of the team this position is for.
		/// </summary>
		public string TeamName { get; set; }

		/// <summary>
		/// Gets or sets a list of tags on the position.
		/// </summary>
		public List<Tag> Tags { get; set; }

		/// <summary>
		/// Gets or sets the Employment Type object of the position.
		/// </summary>
		public string EmploymentTypeName { get; set; }

		/// <summary>
		/// Gets or sets the Position Status of the position.
		/// </summary>
		public string PositionStatusName { get; set; }

		/// <summary>
		/// Gets or sets the Position Level object of the position.
		/// </summary>
		public string PositionLevelName { get; set; }
	}
}