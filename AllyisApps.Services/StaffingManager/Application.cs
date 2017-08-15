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
		/// The constructor used when GETTING info from the backend.
		/// <list type="bullet">
		/// <item><description>Includes applicationId.</description></item>
		/// <item><description>Includes UTC dates that are set from the backend.</description></item>
		/// <item><description>Includes applicant and applicationDocument objects set from the backend.</description></item>
		/// </list>
		/// </summary>
		/// <param name="applicationId">The application id.</param>
		/// <param name="applicantId">The applicant that wrote the application.</param>
		/// <param name="positionId">The position that this application is for.</param>
		/// <param name="applicationStatus">The <see cref="ApplicationStatusEnum"/> (in progress, accepted, rejected).</param>
		/// <param name="notes">Application notes.</param>
		/// <param name="applicationCreatedUtc">Created date.</param>
		/// <param name="applicationModifiedUtc">Modified date.</param>
		/// <param name="applicationDocuments">A list of <see cref="ApplicationDocument"/>.</param>
		/// <param name="applicant"><see cref="Services.Applicant"/> containing info about the application's author.</param>
		public Application(
			int applicationId,
			int applicantId,
			int positionId,
			ApplicationStatusEnum applicationStatus,
			string notes,
			DateTime applicationCreatedUtc,
			DateTime applicationModifiedUtc,
			IEnumerable<ApplicationDocument> applicationDocuments,
			Applicant applicant)
		{
			ApplicationId = applicationId;
			ApplicantId = applicantId;
			PositionId = positionId;
			ApplicationStatus = applicationStatus;
			ApplicationCreatedUtc = applicationCreatedUtc;
			ApplicationModifiedUtc = applicationModifiedUtc;
			Notes = notes;
			ApplicationDocuments = applicationDocuments;
			Applicant = applicant;
		}

		/// <summary>
		/// The constructor used when INSERTING into the backend.
		/// <list type="bullet">
		/// <item><description>Excludes applicationId.</description></item>
		/// <item><description>Excludes UTC dates -- they are set from the backend.</description></item>
		/// <item><description>Excludes applicant and applicationDocument objects  -- they are set from the backend.</description></item>
		/// </list>
		/// </summary>
		/// <param name="applicantId">The applicant that wrote the application.</param>
		/// <param name="positionId">The position that this application is for.</param>
		/// <param name="applicationStatusId">The applicationStatus id (in progress, accepted, rejected).</param>
		/// <param name="notes">Application notes.</param>
		public Application(
			int applicantId,
			int positionId,
			ApplicationStatusEnum applicationStatusId,
			string notes)
		{
			ApplicantId = applicantId;
			PositionId = positionId;
			ApplicationStatus = applicationStatus;
			Notes = notes;
		}

		/// <summary>
		/// The constructor used when UPDATING into the backend.
		/// <list type="bullet">
		/// <item><description>Includes applicationId.</description></item>
		/// <item><description>Excludes UTC dates -- they are set from the backend.</description></item>
		/// <item><description>Excludes applicant and applicationDocument objects  -- they are set from the backend.</description></item>
		/// </list>
		/// </summary>
		/// <param name="applicationId">The application id.</param>
		/// <param name="applicantId">The applicant that wrote the application.</param>
		/// <param name="positionId">The position that this application is for.</param>
		/// <param name="applicationStatusId">The applicationStatus id (in progress, accepted, rejected).</param>
		/// <param name="notes">Application notes.</param>
		public Application(
			int applicationId,
			int applicantId,
			int positionId,
			ApplicationStatusEnum applicationStatusId,
			string notes)
		{
			ApplicationId = applicationId;
			ApplicantId = applicantId;
			PositionId = positionId;
			ApplicationStatus = applicationStatus;
			Notes = notes;
		}

		/// <summary>
		/// Gets or sets the application's ID.
		/// </summary>
		public int ApplicationId
		{
			get
			{
				return applicationId;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ApplicationId", value, "ApplicationId must be greater than 0.");
				}
				applicationId = value;
			}
		}

		/// <summary>
		/// Gets or sets the applicant's ID.
		/// </summary>
		public int ApplicantId
		{
			get
			{
				return applicantId;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ApplicantId", value, "ApplicantId must be greater than 0.");
				}
				applicantId = value;
			}
		}

		/// <summary>
		/// Gets or sets the position's ID.
		/// </summary>
		public int PositionId
		{
			get
			{
				return positionId;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("PositionId", value, "PositionId must be greater than 0.");
				}
				positionId = value;
			}
		}

		/// <summary>
		/// Gets or sets the application status ID.
		/// </summary>
		public ApplicationStatusEnum ApplicationStatus
		{
			get
			{
				return applicationStatus;
			}
			set
			{
				if (!Enum.IsDefined(typeof(ApplicationStatusEnum), value))
				{
					throw new ArgumentOutOfRangeException("ApplicationStatus", value, "ApplicationStatus is not a valid enum value.");
				}
				applicationStatus = value;
			}
		}

		/// <summary>
		/// Gets DateCreated.
		/// </summary>
		public DateTime ApplicationCreatedUtc { get; }

		/// <summary>
		/// Gets date modified.
		/// </summary>
		public DateTime ApplicationModifiedUtc { get; }

		/// <summary>
		/// Gets or sets Notes.
		/// </summary>
		public string Notes { get; set; }

		/// <summary>
		/// Gets the ApplicationDocument list.
		/// </summary>
		public IEnumerable<ApplicationDocument> ApplicationDocuments { get; }

		/// <summary>
		/// Gets the ApplicantDBEntity.
		/// </summary>
		public Applicant Applicant { get; }
	}
}
