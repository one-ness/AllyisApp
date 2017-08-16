using System;
using System.Collections.Generic;

namespace AllyisApps.Services.StaffingManager
{
	public class Position
	{
		private DateTime? startDate;
		private int positionStatusId;
		private string positionTitle;
		private int durationMonths;
		private int employmentTypeId;
		private int positionCount;
		private string requiredSkills;
		private string hiringManager;
		private string teamName;
		private List<Tag> tags;

		/// <summary>
		/// This contructor for use by DBEntity to be passed to view
		/// </summary>
		/// <param name="positionId"></param>
		/// <param name="organizationId"></param>
		/// <param name="customerId"></param>
		/// <param name="addressId"></param>
		/// <param name="positionCreatedUtc"></param>
		/// <param name="positionModifiedUtc"></param>
		/// <param name="startDate"></param>
		/// <param name="positionStatusId"></param>
		/// <param name="positionTitle"></param>
		/// <param name="billingRateFrequency"></param>
		/// <param name="billingRateAmount"></param>
		/// <param name="durationMonths"></param>
		/// <param name="employmentTypeId"></param>
		/// <param name="positionCount"></param>
		/// <param name="requiredSkills"></param>
		/// <param name="jobResponsibilities"></param>
		/// <param name="desiredSkills"></param>
		/// <param name="positionLevelId"></param>
		/// <param name="hiringManager"></param>
		/// <param name="teamName"></param>
		/// <param name="address"></param>
		/// <param name="city"></param>
		/// <param name="state"></param>
		/// <param name="country"></param>
		/// <param name="postalCode"></param>
		/// <param name="tags"></param>
		public Position(
			int organizationId,
			int customerId,
			int addressId,
			int positionStatusId,
			string positionTitle,
			int durationMonths,
			int employmentTypeId,
			int positionCount,
			string requiredSkills,
			int positionLevelId,
			string address,
			string city,
			string state,
			string country,
			string postalCode,
			int? positionId = null,
			DateTime? positionCreatedUtc = null,
			DateTime? positionModifiedUtc = null,
			DateTime? startDate = null,
			int? billingRateFrequency = null,
			int? billingRateAmount = null,
			string jobResponsibilities = null,
			string desiredSkills = null,
			List<Tag> tags = null,
			string hiringManager = null,
			string teamName = null)
		{
			PositionId = positionId;
			OrganizationId = organizationId;
			CustomerId = customerId;
			AddressId = addressId;
			this.PositionCreatedUtc = positionCreatedUtc;
			this.PositionModifiedUtc = positionModifiedUtc;
			StartDate = startDate;
			PositionStatusId = positionStatusId;
			PositionTitle = positionTitle;
			BillingRateFrequency = billingRateFrequency;
			BillingRateAmount = billingRateAmount;
			DurationMonths = durationMonths;
			EmploymentTypeId = employmentTypeId;
			PositionCount = positionCount;
			RequiredSkills = requiredSkills;
			JobResponsibilities = jobResponsibilities;
			DesiredSkills = desiredSkills;
			PositionLevelId = positionLevelId;
			HiringManager = hiringManager;
			TeamName = teamName;
			Address = address;
			City = city;
			State = state;
			Country = country;
			PostalCode = postalCode;
			Tags = tags;
		}

		/// <summary>
		/// Gets or sets the position's Id
		/// </summary>
		public int? PositionId { get; set; }

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
		/// Gets when the position was created
		/// </summary>
		public DateTime? PositionCreatedUtc { get; set; }

		/// <summary>
		/// Gets when the position was last modified
		/// </summary>
		public DateTime? PositionModifiedUtc { get; set; }

		/// <summary>
		/// Get or sets the Start date of the position
		/// </summary>
		public DateTime? StartDate
		{
			get => startDate;
			set
			{
				if (value == null) throw new ArgumentNullException("StartDate", "The Start Date cannot be blank");
				startDate = value;
			}
		}

		/// <summary>
		/// Gets or sets the status int(enum) of the position
		/// </summary>
		public int PositionStatusId
		{
			get => positionStatusId;
			set
			{
				if (!Enum.IsDefined(typeof(PositionStatusEnum), value)) throw new ArgumentOutOfRangeException("PositionStatusId", value, "Position Status was not defined");
				positionStatusId = value;
			}
		}

		/// <summary>
		/// Gets or sets the position name
		/// </summary>
		public string PositionTitle
		{
			get => positionTitle;
			set
			{
				if (value.Length > 140 || value.Length <= 0) throw new ArgumentOutOfRangeException("PositionTitle", value, "The position title cannot be blank or over 140 characters");
				positionTitle = value;
			}
		}

		/// <summary>
		/// Gets or sets billing rate frequency (eg: Months, Weeks)
		/// </summary>
		public int? BillingRateFrequency { get; set; }

		/// <summary>
		/// Get or sets the billing rate amount in dollars
		/// </summary>
		public int? BillingRateAmount { get; set; }

		/// <summary>
		/// Gets or sets the duration of the position if applicable
		/// </summary>
		public int DurationMonths
		{
			get => durationMonths;
			set
			{
				if (value < 1) throw new ArgumentOutOfRangeException("DurationMonths", value, "The Month Duration cannot be less than 1 Month");
				durationMonths = value;
			}
		}

		/// <summary>
		/// Gets or sets the employment type (eg: salary, hourly)
		/// </summary>
		public int EmploymentTypeId
		{
			get => employmentTypeId;
			set
			{
				if (value <= 0) throw new ArgumentOutOfRangeException("EmploymentTypeId", value, "Employment Type cannot be 0");
				employmentTypeId = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of hires needed for this position
		/// </summary>
		public int PositionCount
		{
			get => positionCount;
			set
			{
				if (value < 0) throw new ArgumentOutOfRangeException("PositionCount", value, "Position Count cannot be less than 0");
				positionCount = value;
			}
		}

		/// <summary>
		/// Get or sets the Required Skills description
		/// </summary>
		public string RequiredSkills
		{
			get => requiredSkills;
			set
			{
				if (string.IsNullOrEmpty(value)) throw new ArgumentOutOfRangeException("RequiredSkills", value, "Required Skills cannot be left blank");
				requiredSkills = value;
			}
		}

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
		public int PositionLevelId { get; set; }

		/// <summary>
		/// Get or sets the name of the responsible hiring manager
		/// </summary>
		public string HiringManager
		{
			get => hiringManager;
			set
			{
				if (value.Length > 140) throw new ArgumentOutOfRangeException("HiringManager", value, "Hiring Manager Name cannot be over 140 characters");
				hiringManager = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the team this position is for
		/// </summary>
		public string TeamName
		{
			get => teamName;
			set
			{
				if (value.Length > 140) throw new ArgumentOutOfRangeException("TeamName", value, "Team Name cannot be  over 140 characters");
				teamName = value;
			}
		}

		/// <summary>
		/// Gets or sets the address for the positions
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// Gets or sets the city of the positions address
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the state of the positions address
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the country of the positions address
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the postal code of the positions address
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets a list of tags on the position
		/// </summary>
		public List<Tag> Tags
		{
			get => tags;
			set
			{
				if (value.Count > 10) throw new ArgumentOutOfRangeException("Tags", value, "A position can only have a maximum of 10 tags");
				tags = value;
			}
		}

	}
}
