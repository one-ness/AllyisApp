using AllyisApps.Services.Lookup;
using System;
using System.Collections.Generic;

namespace AllyisApps.Services.StaffingManager
{
	public class Position
	{
		private const int titleLengthMax = 140;
		private const int nameLenthMax = 64;
		private const int maxTags = 10;
		
		private string positionTitle;
		private int employmentTypeId;
		private int positionCount;
		private string requiredSkills;
		private string hiringManager;
		private string teamName;
		private List<Tag> tags;

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
		/// Gets or sets the address of the position location.
		/// </summary>
		public int AddressId { get; set; }

		/// <summary>
		/// Gets when the position was created.
		/// </summary>
		public DateTime PositionCreatedUtc { get; set; }

		/// <summary>
		/// Gets when the position was last modified.
		/// </summary>
		public DateTime PositionModifiedUtc { get; set; }

		/// <summary>
		/// Get or sets the Start date of the position.
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets or sets the id int of the positionStatus.
		/// </summary>
		public int PositionStatusId { get; set; }

		/// <summary>
		/// Gets or sets the position name.
		/// </summary>
		public string PositionTitle
		{
			get => positionTitle;
			set
			{
				if (value.Length > titleLengthMax || value.Length <= 0) throw new ArgumentOutOfRangeException(nameof(positionTitle), value, "The position title cannot be blank or over " + titleLengthMax.ToString() + " characters");
				positionTitle = value;
			}
		}

		/// <summary>
		/// Gets or sets billing rate frequency (eg: Months, Weeks).
		/// </summary>
		public int BillingRateFrequency { get; set; }

		/// <summary>
		/// Get or sets the billing rate amount in dollars.
		/// </summary>
		public int BillingRateAmount { get; set; }

		/// <summary>
		/// Gets or sets the duration of the position if applicable.
		/// </summary>
		public int DurationMonths { get; set; }

	/// <summary>
	/// Gets or sets the employment type (eg: salary, hourly).
	/// </summary>
	public int EmploymentTypeId
		{
			get => employmentTypeId;
			set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException(nameof(employmentTypeId), value, "Employment Type cannot be 0");
				employmentTypeId = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of hires needed for this position.
		/// </summary>
		public int PositionCount
		{
			get => positionCount;
			set
			{
				if (value < 0) throw new ArgumentOutOfRangeException(nameof(positionCount), value, "Position Count cannot be less than 0");
				positionCount = value;
			}
		}

		/// <summary>
		/// Get or sets the Required Skills description.
		/// </summary>
		public string RequiredSkills
		{
			get => requiredSkills;
			set
			{
				if (string.IsNullOrEmpty(value)) throw new ArgumentOutOfRangeException(nameof(requiredSkills), value, "Required Skills cannot be left blank");
				requiredSkills = value;
			}
		}

		/// <summary>
		/// Gets or sets the Job responibilites description.
		/// </summary>
		public string JobResponsibilities { get; set; }

		/// <summary>
		/// Gets or sets the desired skills description.
		/// </summary>
		public string DesiredSkills { get; set; }

		/// <summary>
		/// Gets or sets the poition level description (eg: Senior, Junior).
		/// </summary>
		public int PositionLevelId { get; set; }

		/// <summary>
		/// Get or sets the name of the responsible hiring manager.
		/// </summary>
		public string HiringManager
		{
			get => hiringManager;
			set
			{
				if (value.Length > nameLenthMax) throw new ArgumentOutOfRangeException(nameof(hiringManager), value, "Hiring Manager Name cannot be over " + nameLenthMax.ToString() + " characters");
				hiringManager = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the team this position is for.
		/// </summary>
		public string TeamName
		{
			get => teamName;
			set
			{
				if (value.Length > nameLenthMax) throw new ArgumentOutOfRangeException(nameof(teamName), value, "Team Name cannot be  over " + nameLenthMax.ToString() + " characters");
				teamName = value;
			}
		}

		/// <summary>
		/// Gets or sets the address for the positions.
		/// </summary>
		public Address Address { get; set; }

		/// <summary>
		/// Gets or sets a list of tags on the position.
		/// </summary>
		public List<Tag> Tags
		{
			get => tags;
			set
			{
				if (value.Count > maxTags) throw new ArgumentOutOfRangeException(nameof(tags), value, "A position can only have a maximum of " + maxTags.ToString() + " tags");
				tags = value;
			}
		}

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
