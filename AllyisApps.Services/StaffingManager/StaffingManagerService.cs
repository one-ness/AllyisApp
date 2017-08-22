//------------------------------------------------------------------------------
// <copyright file="StaffingManagerService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AllyisApps.DBModel.Lookup;
using AllyisApps.DBModel.StaffingManager;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.StaffingManager;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all staffing manager related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		///////////////////////
		// METHOD CONVERTERS //
		///////////////////////

		#region CreateMethods

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

		/// <summary>
		/// Creates a new position.
		/// </summary>
		/// <param name="position">The account object to be created. </param>
		/// <returns>The id of the created position. </returns>
		public int CreatePosition(Position position) => DBHelper.SetupPosition(ServiceObjectToDBEntity(position));

		/// <summary>
		/// Adds an Tag to the DB if there is not already another tag with the same name.
		/// </summary>
		/// <param name="name">The name of the tag to be added to the db. </param>
		/// <param name="positionId">The name of the tag to be added to the db. </param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use. </returns>
		public int CreateTag(string name, int positionId) => DBHelper.CreateTag(name, positionId);

		/// <summary>
		/// Adds a PositionTag to the DB when there is already another tag with the same name.
		/// </summary>
		/// <param name="tagId">The name of the tag to be added to the db. </param>
		/// <param name="positionId">The name of the tag to be added to the db. </param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use. </returns>
		public void AssignTag(int tagId, int positionId) => DBHelper.AssignTag(tagId, positionId);

		#endregion

		#region GetMethods

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

		/// <summary>
		/// Get Position method that pulls a position from the DB.
		/// </summary>
		/// <param name="positionId">ID of position to be pulled.</param>
		/// <returns>returns the service layer position object.</returns>
		public Position GetPosition(int positionId) => GetPositionToPositionServiceObject(DBHelper.GetPositionById(positionId));

		/// <summary>
		/// Get Positions by Org ID method to pull a list of Service Layer positions objects based on their org ID.
		/// </summary>
		/// <param name="organizationId">the tagert organizations ID number. </param>
		/// <returns>A list of service layer Position Objects. </returns>
		public List<Position> GetPositionByOrganizationId(int organizationId) => DBHelper.GetPositionsByOrganizationId(organizationId).Select(GetPositionToPositionServiceObject).ToList();

		/// <summary>
		/// Get Tags by a position Id method; pulls a list of all of the positions tags as service layer Tag Objects.
		/// </summary>
		/// <param name="positionId"> the position whose tags are to be pulled. </param>
		/// <returns>A list of the positions tags as service layer Tag objects</returns>
		public List<Tag> GetTagsByPositionId(int positionId) => DBHelper.GetTagsByPositionId(positionId).Select(DBEntityToServiceObject).ToList();

		/// <summary>
		/// Gets a list of ALL current tags.
		/// </summary>
		/// <returns>returns a list of all current tags. </returns>
		public List<Tag> GetTags() => DBHelper.GetTags().Select(DBEntityToServiceObject).ToList();

		#endregion

		#region UpdateMethods

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

		/// <summary>
		/// Updates the position with the given id.
		/// </summary>
		/// <param name="position">The service layer Position object to be passed to the DB and updated. </param>
		/// <returns>Returns the number of rows updated.</returns>
		public int UpdatePosition(Position position) => DBHelper.UpdatePosition(ServiceObjectToDBEntity(position));

		#endregion

		#region DeleteMethods

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

		/// <summary>
		/// Deletes a tag from the database.
		/// </summary>
		/// <param name="tagId">the id of the Tag to be removed from the db. </param>
		public void DeleteTag(int tagId) => DBHelper.DeleteTag(tagId);

		/// <summary>
		/// Deletes a position tag from the database; Doesnt delete the tag, just removes it from the position.
		/// </summary>
		/// <param name="tagId">tag id of the tag to be removed. </param>
		/// <param name="positionId">the position id that no longer has the tag. </param>
		public void DeletePositionTag(int tagId, int positionId) => DBHelper.DeletePositionTag(tagId, positionId);

		/// <summary>
		/// Removes a Position from the Database.
		/// </summary>
		/// <param name="positionId">ID of the Position to be removed from the DB. </param>
		public void DeletePosition(int positionId) => DBHelper.DeletePosition(positionId);

		#endregion

		//////////////////////////////////////////
		// DB LAYER to SERVICE LAYER CONVERSION //
		//////////////////////////////////////////

		#region DB to Service Conversions

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

		/// <summary>
		/// Converts a DB Layer PositionDBEntity object to a service layer Position Object.
		/// </summary>
		/// <param name="position"> the PositionDBEntity to be converted to service layer object. </param>
		/// <returns>Returns a service layer Position Obejct. </returns>
		public static Position GetPositionToPositionServiceObject(dynamic position)
		{
			if (position == null) throw new ArgumentNullException(nameof(position), "Cannot accept a null position object.");

			return new Position
			{
				OrganizationId = position.OrganizationId,
				CustomerId = position.CustomerId,
				AddressId = position.AddressId,
				PositionStatusId = position.PositionStatusId,
				PositionTitle = position.PositionTitle,
				DurationMonths = position.DurationMonths,
				EmploymentTypeId = position.EmploymentTypeId,
				PositionCount = position.PositionCount,
				RequiredSkills = position.RequiredSkills,
				PositionLevelId = position.PositionLevelId,
				PositionId = position.PositionId,
				PositionCreatedUtc = position.PositionCreatedUtc,
				PositionModifiedUtc = position.PositionModifiedUtc,
				StartDate = position.StartDate,
				BillingRateFrequency = position.BillingRateFrequency,
				BillingRateAmount = position.BillingRateAmount,
				JobResponsibilities = position.JobResponsibilities,
				DesiredSkills = position.DesiredSkills,
				HiringManager = position.HiringManager,
				TeamName = position.TeamName,


				Address = new Address
				{
					Address1 = position.position.Address1,
					Address2 = position.position.Address2,
					City = position.position.City,
					StateName = position.position.StateId,
					CountryName = position.position.CountryCode,
					PostalCode = position.position.PostalCodes
				},

				Tags = DBEntityToServiceObject(position.Tags),

				EmploymentTypeName = position.EmploymentType.EmploymentTypeName,
				PositionLevelName = position.PositionLevel.PositionLevelName,
				PositionStatusName = position.PositionStatus.PositionStatusName
			};
		}

		/// <summary>
		/// Converts a list of TagDBEntity DB layer objects to a list of Tag service layer object.
		/// </summary>
		/// <param name="tags"> The List of tag DB layer objects to be converted. </param>
		/// <returns> Returns a list of Tag service layer objects.  </returns>
		public static List<Tag> DBEntityToServiceObject(List<TagDBEntity> tags)
		{
			var taglist = tags
				.ConvertAll(x => new Tag { TagId = x.TagId, TagName = x.TagName });
			return taglist;
		}

		/// <summary>
		/// Converts a TagDBEntity DB layer object to a Tag service layer object.
		/// </summary>
		/// <param name="tag"> The tag DB layer object to be converted. </param>
		/// <returns> Returns a Tag service layer object.  </returns>
		public static Tag DBEntityToServiceObject(TagDBEntity tag)
		{
			if (tag == null) throw new ArgumentNullException(nameof(tag), "Cannot accept a null tag to be converted");

			return new Tag { TagId = tag.TagId, TagName = tag.TagName };
		}

		#endregion DB to Service Conversion

		//////////////////////////////////////////
		// SERVICE LAYER to DB LAYER CONVERSION //
		//////////////////////////////////////////

		#region Service to DB Conversions

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

		/// <summary>
		/// Converts a Position service layer object to a PositionDBEntity object.
		/// </summary>
		/// <param name="position"> The Position service layer obejct. </param>
		/// <returns> Returns the PositionDBEntity for the DB layer.  </returns>
		public static dynamic ServiceObjectToDBEntity(Position position)
		{
			if (position == null) throw new ArgumentNullException(nameof(position), "Cannot accept a null position object");

			dynamic obj = new ExpandoObject();

			obj.Position = new ExpandoObject();
			obj.Position.OrganizationId = position.OrganizationId;
			obj.Position.CustomerId = position.CustomerId;
			obj.Position.AddressId = position.AddressId;
			obj.Position.PositionStatusId = position.PositionStatusId;
			obj.Position.ositionTitle = position.PositionTitle;
			obj.Position.DurationMonths = position.DurationMonths;
			obj.Position.EmploymentTypeId = position.EmploymentTypeId;
			obj.Position.PositionCount = position.PositionCount;
			obj.Position.RequiredSkills = position.RequiredSkills;
			obj.Position.PositionLevelId = position.PositionLevelId;
			obj.Position.PositionId = position.PositionId;
			obj.Position.PositionCreatedUtc = position.PositionCreatedUtc;
			obj.Position.PositionModifiedUtc = position.PositionModifiedUtc;
			obj.Position.StartDate = position.StartDate;
			obj.Position.BillingRateFrequency = position.BillingRateFrequency;
			obj.Position.BillingRateAmount = position.BillingRateAmount;
			obj.Position.JobResponsibilities = position.JobResponsibilities;
			obj.Position.DesiredSkills = position.DesiredSkills;
			obj.Position.HiringManager = position.HiringManager;
			obj.Position.TeamName = position.TeamName;

			obj.Address = new ExpandoObject();
			obj.Address.Address1 = position.Address.Address1;
			obj.Address.Address2 = position.Address.Address2;
			obj.Address.City = position.Address.City;
			obj.Address.StateId = position.Address.StateId;
			obj.Address.PostalCode = position.Address.PostalCode;
			obj.Address.CountryCode = position.Address.CountryCode;

			obj.Tags = ServiceObjectToDBEntity(position.Tags);

			return obj;
		}


		/// <summary>
		/// Converts a list of Tag service layer objects to a TagDBEntity object list.
		/// </summary>
		/// <param name="tags"> The List of tag service layer objects to be converted. </param>
		/// <returns> Returns a list of TagDBEntity objects for the DB layer.  </returns>
		public static List<dynamic> ServiceObjectToDBEntity(List<Tag> tags)
		{
			if (tags == null) throw new ArgumentNullException(nameof(tags), "Cannot accept null list of tags to be converted.");

			dynamic obj = tags;

			return obj;
		}

		/// <summary>
		/// Converts a Tag service layer object to a TagDBEntity object.
		/// </summary>
		/// <param name="tag"> Thetag service layer object to be converted. </param>
		/// <returns> Returns a TagDBEntity object for the DB layer.  </returns>
		public static TagDBEntity ServiceObjectToDBEntity(Tag tag)
		{
			if (tag == null) throw new ArgumentNullException(nameof(tag), "Cannot accept a null tag to be converted.");

			return new TagDBEntity { TagId = (int)tag.TagId, TagName = tag.TagName };
		}

		#endregion Service to DB Conversions
	}
}
