using System;
using System.Collections.Generic;

namespace AllyisApps.DBModel.StaffingManager
{
	/// <summary>
	/// Represents the Address table in the database.
	/// </summary>
	public class PositionDBEntity
	{
		/// <summary>
		/// Gets or sets the position's Id
		/// </summary>
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets positions associated organization
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets positionss hiring customer
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets or sets the address of the position location
		/// </summary>
		public int AddressId { get; set; }

		/// <summary>
		/// Get or sets the Start date of the position
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the status int(enum) of the position
		/// </summary>
		public int PositionStatus { get; set; }

		/// <summary>
		/// Gets or sets the position name
		/// </summary>
		/// 
		public string PositionTitle { get; set; }
		/// <summary>
		/// Gets or sets billing rate frequency (eg: Months, Weeks)
		/// </summary>
		public int BillingRateFrequency { get; set; }

		/// <summary>
		/// Get or sets the billing rate amount in dollars
		/// </summary>
		public int BillingRateAmount { get; set; }

		/// <summary>
		/// Gets or sets the duration of the position if applicable
		/// </summary>
		public int DurationMonths { get; set; }

		/// <summary>
		/// Gets or sets the employment type (eg: salary, hourly)
		/// </summary>
		public int EmploymentType { get; set; }

		/// <summary>
		/// Gets or sets the number of hires needed for this position
		/// </summary>
		public int PositionCount { get; set; }

		/// <summary>
		/// Get or sets the Required Skills description
		/// </summary>
		public string RequiredSkills { get; set; }

		/// <summary>
		/// Gets or sets the Job responibilites description
		/// </summary>
		public string JobResponsibilities { get; set; }

		/// <summary>
		/// Gets or sets the desired skills description
		/// </summary>
		public string DesiredSkills { get; set; }

		/// <summary>
		/// Gets or sets the poition level description (eg: Senior, Junior)
		/// </summary>
		public string PositionLevel { get; set; }

		/// <summary>
		/// Get or sets the name of the responsible hiring manager
		/// </summary>
		public string HiringManager { get; set; }

		/// <summary>
		/// Gets or sets the name of the team this position is for
		/// </summary>
		public string TeamName { get; set; }

		/// <summary>
		/// Gets or sets a list of tags on the position
		/// </summary>
		public IEnumerable<TagDBEntity> Tags { get; set; }

	}
}
