//------------------------------------------------------------------------------
// <copyright file="StaffingManagerService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AllyisApps.DBModel.StaffingManager;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all staffing manager related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		////////////////////////////
		/*         CREATE         */
		////////////////////////////

		/// <summary>
		/// Adds an applicant to the DB if there is not already another applicant with the same email.
		/// </summary>
		/// <param name="applicant">The applicant object to be added to the db.</param>
		/// <returns>The id of the created address and applicant</returns>
		public Tuple<int, int> CreateApplicant(Applicant applicant) => DBHelper.CreateApplicant(ServiceObjectToDBEntity(applicant));

		/// <summary>
		/// Adds an application to the DB.
		/// </summary>
		/// <param name="application">The application object to be added to the db.</param>
		/// <returns>The id of the created application</returns>
		public int CreateApplication(Application application) => DBHelper.CreateApplication(ServiceObjectToDBEntity(application));

		/// <summary>
		/// Adds an application document to the DB, which joins to an application.
		/// </summary>
		/// <param name="applicationDocument">The application document object to be added to the db.</param>
		/// <returns>The id of the created application document</returns>
		public int CreateApplicationDocument(ApplicationDocument applicationDocument) => DBHelper.CreateApplicationDocument(ServiceObjectToDBEntity(applicationDocument));


		////////////////////////////
		/*          READ          */
		////////////////////////////

		/// <summary>
		/// Retrieves the applicant with a given id.
		/// </summary>
		/// <param name="applicantId">The id of the applicant.</param>
		/// <returns>One applicant, if present.</returns>
		public Applicant GetApplicantById(int applicantId) => DBEntityToServiceObject(DBHelper.GetApplicantById(applicantId));

		/// <summary>
		/// Retrieves the application with a given id.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>One application, if present.</returns>
		public Application GetApplicationById(int applicationId) => DBEntityToServiceObject(DBHelper.GetApplicationById(applicationId));

		/// <summary>
		/// Retrieves the application document with a given id.
		/// </summary>
		/// <param name="applicationDocumentId">The id of the application document.</param>
		/// <returns>One application document, if present.</returns>
		public ApplicationDocument GetApplicationDocumentById(int applicationDocumentId) => DBEntityToServiceObject(DBHelper.GetApplicationDocumentById(applicationDocumentId));

		/// <summary>
		/// Retrieves the applicant that submitted the given application.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>The applicant that submitted the given application.</returns>
		public Applicant GetApplicantByApplicationId(int applicationId) => DBEntityToServiceObject(DBHelper.GetApplicantByApplicationId(applicationId));

		/// <summary>
		/// Retrieves all applications **and associated application information** for a given position.
		/// </summary>
		/// <param name="positionId">The id of the position.</param>
		/// <returns>
		/// List of application objects containing:
		///  - Application info
		///  - Applicant info
		///  - Application document info
		/// </returns>
		public List<Application> GetApplicationsInfoByPositionId(int positionId) => DBHelper.GetApplicationsInfoByPositionId(positionId).Select(DBEntityToServiceObject).ToList();

		/// <summary>
		/// Retrieves all applications that have been submitted by the given applicant.
		/// </summary>
		/// <param name="applicantId">The id of the applicant.</param>
		/// <returns>All applications that have been submitted by the given applicant.</returns>
		public List<Application> GetApplicationsByApplicantId(int applicantId) => DBHelper.GetApplicationsByApplicantId(applicantId).Select(DBEntityToServiceObject).ToList();

		/// <summary>
		/// Retrieves all application documents attached to the given application.
		/// </summary>
		/// <param name="applicationId">The id of the application, containing multiple application documents.</param>
		/// <returns>All application documents attached to the given application.</returns>
		public List<ApplicationDocument> GetApplicationDocumentsByApplicationId(int applicationId) => DBHelper.GetApplicationDocumentsByApplicationId(applicationId).Select(DBEntityToServiceObject).ToList();


		////////////////////////////
		/*         UPDATE         */
		////////////////////////////

		/// <summary>
		/// Updates the given applicant if there is not already another applicant with the same email.
		/// </summary>
		/// <param name="applicant">The applicant object to be updated.</param>
		/// <returns>The number of rows updated.</returns>
		public int UpdateApplicant(Applicant applicant) => DBHelper.UpdateApplicant(ServiceObjectToDBEntity(applicant));

		/// <summary>
		/// Updates the given application.
		/// </summary>
		/// <param name="application">The application object to be updated.</param>
		/// <returns>The number of rows updated.</returns>
		public int UpdateApplication(Application application) => DBHelper.UpdateApplication(ServiceObjectToDBEntity(application));

		/// <summary>
		/// Updates the given application document.
		/// </summary>
		/// <param name="applicationDocument">The application document object to be updated.</param>
		/// <returns>The number of rows updated.</returns>
		public int UpdateApplicationDocument(ApplicationDocument applicationDocument) => DBHelper.UpdateApplicationDocument(ServiceObjectToDBEntity(applicationDocument));


		////////////////////////////
		/*         DELETE         */
		////////////////////////////

		/// <summary>
		/// Deletes an applicant from the database
		/// </summary>
		/// <param name="applicantId">The applicant to be deleted</param>
		public void DeleteApplicant(int applicantId) => DBHelper.DeleteApplicant(applicantId);

		/// <summary>
		/// Deletes an application from the database
		/// </summary>
		/// <param name="applicationId">The applicant to be deleted</param>
		public void DeleteApplication(int applicationId) => DBHelper.DeleteApplication(applicationId);

		/// <summary>
		/// Deletes an application document from the database
		/// </summary>
		/// <param name="applicationDocumentId">The applicant to be deleted</param>
		public void DeleteApplicationDocument(int applicationDocumentId) => DBHelper.DeleteApplicationDocument(applicationDocumentId);

		#region Object Conversions

		public static Applicant DBEntityToServiceObject(ApplicantDBEntity applicant)
		{
			if (applicant == null)
			{
				throw new ArgumentNullException(nameof(applicant), nameof(applicant) + " must not be null.");
			}

			return new Applicant
			{
				Address = applicant.Address,
				State = applicant.State,
				AddressId = applicant.AddressId,
				ApplicantId = applicant.ApplicantId,
				City = applicant.City,
				Country = applicant.Country,
				Email = applicant.Email,
				FirstName = applicant.FirstName,
				LastName = applicant.LastName,
				Notes = applicant.Notes,
				PhoneNumber = applicant.PhoneNumber,
				PostalCode = applicant.PostalCode
			};
		}

		public static Application DBEntityToServiceObject(ApplicationDBEntity application)
		{
			if (application == null)
			{
				throw new ArgumentNullException(nameof(application), nameof(application) + " must not be null.");
			}

			return new Application
			{
				ApplicantId = application.ApplicantId,
				Notes = application.Notes,
				Applicant = DBEntityToServiceObject(application.Applicant),
				ApplicationCreatedUtc = application.ApplicationCreatedUtc,
				ApplicationDocuments = application.ApplicationDocuments.Select(DBEntityToServiceObject).ToList(),
				ApplicationId = application.ApplicationId,
				ApplicationModifiedUtc = application.ApplicationModifiedUtc,
				ApplicationStatus = (ApplicationStatusEnum)application.ApplicationStatusId,
				PositionId = application.PositionId
			};
		}

		public static ApplicationDocument DBEntityToServiceObject(ApplicationDocumentDBEntity applicationDocument)
		{
			if (applicationDocument == null)
			{
				throw new ArgumentNullException(nameof(applicationDocument), nameof(applicationDocument) + " must not be null.");
			}

			return new ApplicationDocument
			{
				ApplicationId = applicationDocument.ApplicationId,
				ApplicationDocumentId = applicationDocument.ApplicationDocumentId,
				DocumentLink = applicationDocument.DocumentLink,
				DocumentName = applicationDocument.DocumentName
			};
		}

		public static ApplicationDBEntity ServiceObjectToDBEntity(Application application)
		{
			if (application == null)
			{
				throw new ArgumentNullException(nameof(application), nameof(application) + " must not be null.");
			}

			return new ApplicationDBEntity
			{
				ApplicationId = application.ApplicationId,
				ApplicationStatusId = (int)application.ApplicationStatus,
				Notes = application.Notes,
				PositionId = application.PositionId,
				ApplicantId = application.ApplicantId
			};
		}

		public static ApplicationDocumentDBEntity ServiceObjectToDBEntity(ApplicationDocument applicationDocument)
		{
			if (applicationDocument == null)
			{
				throw new ArgumentNullException(nameof(applicationDocument), nameof(applicationDocument) + " must not be null.");
			}

			return new ApplicationDocumentDBEntity
			{
				ApplicationId = applicationDocument.ApplicationId,
				ApplicationDocumentId = applicationDocument.ApplicationDocumentId,
				DocumentName = applicationDocument.DocumentName,
				DocumentLink = applicationDocument.DocumentLink
			};
		}

		public static ApplicantDBEntity ServiceObjectToDBEntity(Applicant applicant)
		{
			if (applicant == null)
			{
				throw new ArgumentNullException(nameof(applicant), nameof(applicant) + " must not be null.");
			}

			return new ApplicantDBEntity
			{
				Address = applicant.Address,
				State = applicant.State,
				AddressId = applicant.AddressId,
				ApplicantId = applicant.ApplicantId,
				City = applicant.City,
				Country = applicant.Country,
				Email = applicant.Email,
				FirstName = applicant.FirstName,
				LastName = applicant.LastName,
				Notes = applicant.Notes,
				PhoneNumber = applicant.PhoneNumber,
				PostalCode = applicant.PostalCode
			};
		}

		#endregion Object Conversions
	}
}
