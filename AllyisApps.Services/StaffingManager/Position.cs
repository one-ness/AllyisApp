using System;
using System.Collections.Generic;

namespace AllyisApps.Services.StaffingManager
{
	public class Position
	{
		private int? positionId;
		private int organizationId;
		private int customerId;
		private int addressId;
		private DateTime? positionCreatedUtc;
		private DateTime? positionModifiedUtc;
		private DateTime? startDate;
		private int positionStatusId;
		private string positionTitle;
		private int? billingRateFrequency;
		private int? billingRateAmount;
		private int durationMonths;
		private int employmentTypeId;
		private int positionCount;
		private string requiredSkills;
		private string jobResponsibilities;
		private string desiredSkills;
		private int positionLevelId;
		private string hiringManager;
		private string teamName;
		private string address;
		private string city;
		private string state;
		private string country;
		private string postalCode;
		private IEnumerable<Tag> tags;

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
			IEnumerable<Tag> tags = null,
			string hiringManager = null, 
			string teamName = null)
		{
			PositionId = positionId;
			OrganizationId = organizationId;
			CustomerId = customerId;
			AddressId = addressId;
			this.positionCreatedUtc = positionCreatedUtc;
			this.positionModifiedUtc = positionModifiedUtc;
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
		public int? PositionId
		{
			get { return positionId; }
			set { positionId = value; }
		}

		/// <summary>
		/// Gets or sets positions associated organization
		/// </summary>
		public int OrganizationId
		{
			get { return organizationId; }
			set { organizationId = value; }
		}

		/// <summary>
		/// Gets or sets positionss hiring customer
		/// </summary>
		public int CustomerId
		{
			get { return customerId; }
			set { customerId = value; }
		}

		/// <summary>
		/// Gets or sets the address of the position location
		/// </summary>
		public int AddressId
		{
			get { return addressId; }
			set { addressId = value; }
		}

		/// <summary>
		/// Gets when the position was created
		/// </summary>
		public DateTime? PositionCreatedUtc
		{
			get { return positionCreatedUtc; }
			set { positionCreatedUtc = value; }
		}

		/// <summary>
		/// Gets when the position was last modified
		/// </summary>
		public DateTime? PositionModifiedUtc
		{
			get { return positionModifiedUtc; }
			set { positionModifiedUtc = value; }
		}

		/// <summary>
		/// Get or sets the Start date of the position
		/// </summary>
		public DateTime? StartDate
		{
			get
			{
				return startDate;
			}
			set
			{
				if(value == null) throw new ArgumentNullException("StartDate", "The Start Date cannot be blank");
				value = startDate;
			}
		}

		/// <summary>
		/// Gets or sets the status int(enum) of the position
		/// </summary>
		public int PositionStatusId
		{
			get
			{
				return positionStatusId;
			}
			set
			{
				if (!Enum.IsDefined(typeof(PositionStatusEnum), value)) throw new ArgumentOutOfRangeException("PositionStatusId", value, "Position Status was not defined");
			}
		}

		/// <summary>
		/// Gets or sets the position name
		/// </summary>
		public string PositionTitle
		{
			get
			{
				return PositionTitle;
			}
			set
			{
				if (value.Length > 140 || value.Length <= 0) throw new ArgumentOutOfRangeException("PositionTitle", value, "The position title cannot be blank or over 140 characters");
				value = positionTitle;
			}
		}

		/// <summary>
		/// Gets or sets billing rate frequency (eg: Months, Weeks)
		/// </summary>
		public int? BillingRateFrequency
		{
			get { return billingRateFrequency; }
			set { billingRateFrequency = value; }
		}

		/// <summary>
		/// Get or sets the billing rate amount in dollars
		/// </summary>
		public int? BillingRateAmount
		{
			get { return billingRateAmount; }
			set { billingRateAmount = value; }
		}

		/// <summary>
		/// Gets or sets the duration of the position if applicable
		/// </summary>
		public int DurationMonths
		{
			get
			{
				return durationMonths;
			}
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
			get { return employmentTypeId; }
			set
			{
				if (value <= 0 ) throw new ArgumentOutOfRangeException("EmploymentTypeId", value, "Employment Type cannot be 0");
				employmentTypeId = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of hires needed for this position
		/// </summary>
		public int PositionCount
		{
			get
			{
				return positionCount;
			}
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
			get { return requiredSkills; }
			set
			{
				if(value == null) throw new ArgumentOutOfRangeException("RequiredSkills", value, "Required Skills cannot be left blank");
				requiredSkills = value;
			}
		}

		/// <summary>
		/// Gets or sets the Job responibilites description
		/// </summary>
		public string JobResponsibilities
		{
			get { return jobResponsibilities; }
			set { jobResponsibilities = value; }
		}

		/// <summary>
		/// Gets or sets the desired skills description
		/// </summary>
		public string DesiredSkills
		{
			get { return desiredSkills; }
			set { desiredSkills = value; }
		}

		/// <summary>
		/// Gets or sets the poition level description (eg: Senior, Junior)
		/// </summary>
		public int PositionLevelId
		{
			get { return positionLevelId; }
			set { positionLevelId = value; }
		}

		/// <summary>
		/// Get or sets the name of the responsible hiring manager
		/// </summary>
		public string HiringManager
		{
			get
			{
				return hiringManager;
			}
			set
			{
				if (value.Length > 140) throw new ArgumentOutOfRangeException("HiringManager", value, "Hiring Manager Name cannot be over 140 characters");
				hiringManager = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the team this position is for
		/// </summary>
		public string TeamName {
			get
			{
				return teamName;
			}
			set
			{
				if(value.Length > 140) throw new ArgumentOutOfRangeException("TeamName", value, "Team Name cannot be  over 140 characters");
				teamName = value;
			}
		}

		/// <summary>
		/// Gets or sets the address for the positions
		/// </summary>
		public string Address
		{
			get { return address; }
			set { address = value; }
		}

		/// <summary>
		/// Gets or sets the city of the positions address
		/// </summary>
		public string City
		{
			get { return city; }
			set { city = value; }
		}

		/// <summary>
		/// Gets or sets the state of the positions address
		/// </summary>
		public string State
		{
			get { return state; }
			set { state = value; }
		}

		/// <summary>
		/// Gets or sets the country of the positions address
		/// </summary>
		public string Country
		{
			get { return country; }
			set { country = value; }
		}

		/// <summary>
		/// Gets or sets the postal code of the positions address
		/// </summary>
		public string PostalCode
		{
			get { return postalCode; }
			set { postalCode = value; }
		}

		/// <summary>
		/// Gets or sets a list of tags on the position
		/// </summary>
		public IEnumerable<Tag> Tags
		{ 
			get { return tags; }
			set { tags = value; }
		}

	}
}
