//------------------------------------------------------------------------------
// <copyright file="Application.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Services
{
	/// <summary>
	/// An Application object, holding all info directly related to the Application.  Also performs basic field validation.
	/// </summary>
	public class Application
	{
		private int applicationId;
		private int applicantId;
		private int positionId;
		private ApplicationStatusEnum applicationStatus;

		/// <summary>
		/// Gets or sets the application's ID.
		/// </summary>
		public int ApplicationId
		{
			get => applicationId;
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(ApplicationId), value, nameof(ApplicationId) + " must be greater than 0.");
				}
				applicationId = value;
			}
		}

		/// <summary>
		/// Gets or sets the applicant's ID.
		/// </summary>
		public int ApplicantId
		{
			get => applicantId;
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(ApplicantId), value, nameof(ApplicantId) + " must be greater than 0.");
				}
				applicantId = value;
			}
		}

		/// <summary>
		/// Gets or sets the position's ID.
		/// </summary>
		public int PositionId
		{
			get => positionId;
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(PositionId), value, nameof(PositionId) + " must be greater than 0.");
				}
				positionId = value;
			}
		}

		/// <summary>
		/// Gets or sets the application status ID.
		/// </summary>
		public ApplicationStatusEnum ApplicationStatus
		{
			get => applicationStatus;
			set
			{
				if (!Enum.IsDefined(typeof(ApplicationStatusEnum), value))
				{
					throw new ArgumentOutOfRangeException(nameof(ApplicationStatus), value, nameof(ApplicationStatus) + " is not a valid enum value.");
				}
				applicationStatus = value;
			}
		}

		/// <summary>
		/// Gets DateCreated.
		/// </summary>
		public DateTime ApplicationCreatedUtc { get; set; }

		/// <summary>
		/// Gets date modified.
		/// </summary>
		public DateTime ApplicationModifiedUtc { get; set; }

		/// <summary>
		/// Gets or sets Notes.
		/// </summary>
		public string Notes { get; set; }

		/// <summary>
		/// Gets the ApplicationDocument list.
		/// </summary>
		public List<ApplicationDocument> ApplicationDocuments { get; set; }

		/// <summary>
		/// Gets the ApplicantDBEntity.
		/// </summary>
		public Applicant Applicant { get; set; }
	}
}
