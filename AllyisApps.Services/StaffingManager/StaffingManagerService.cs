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
using AllyisApps.Services.Crm;
using AllyisApps.Services.Lookup;
using AllyisApps.Services.StaffingManager;
using Microsoft.CSharp.RuntimeBinder;
using System.Threading.Tasks;

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
		public int CreateApplicant(Applicant applicant) => DBHelper.CreateApplicant(ServiceObjectToDBEntity(applicant));

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

		/// <summary>
		///
		/// </summary>
		/// <param name="newLevel"></param>
		/// <param name="orgId"></param>
		/// <param name="subId"></param>
		public void CreatePositionLevel(string newLevel, int orgId, int subId)
		{
			dynamic level = new ExpandoObject();
			level.positionLevelName = newLevel;
			level.organizationId = orgId;
			DBHelper.CreatePositionLevel(level);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="newStatus"></param>
		/// <param name="orgId"></param>
		/// <param name="subId"></param>
		public void CreatePositionStatus(string newStatus, int orgId, int subId)
		{
			dynamic status = new ExpandoObject();
			status.positionStatusName = newStatus;
			status.organizationId = orgId;
			DBHelper.CreatePositionStatus(status);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="newType"></param>
		/// <param name="orgId"></param>
		/// <param name="subId"></param>
		public void CreateEmploymentType(string newType, int orgId, int subId)
		{
			dynamic type = new ExpandoObject();
			type.employmentTypeName = newType;
			type.organizationId = orgId;
			DBHelper.CreateEmploymentType(type);
		}

		/// <summary>
		/// Creates a customer.
		/// </summary>
		/// <param name="customer">Customer.</param>
		/// <param name="subscriptionId">.</param>
		/// <returns>Customer id.</returns>
		async public Task<int?> CreateStaffingCustomer(Customer customer, int subscriptionId)
		{
			return await DBHelper.CreateCustomerInfo(GetDBEntitiesFromCustomerInfo(customer));
		}

		#endregion CreateMethods

		#region GetMethods

		/// <summary>
		/// Retrieves the applicants in an organization.
		/// </summary>
		/// <param name="orgId"></param>
		/// <returns>The list of applicants in organization.</returns>
		public List<Applicant> GetApplicantAddressesByOrgId(int orgId) => DBHelper.GetApplicantAddressesBySubscriptionId(orgId).Select(DBApplicantToServiceObject).ToList();

		/// <summary>
		/// Retrieves the applicants in an organization.
		/// </summary>
		/// <param name="orgId"></param>
		/// <returns>The list of applicants in organization.</returns>
		public List<Applicant> GetApplicantsByOrgId(int orgId) => DBHelper.GetApplicantsBySubscriptionId(orgId).Select(DBApplicantToServiceObject).ToList();

		/// <summary>
		/// Retrieves the applicant with a given id.
		/// </summary>
		/// <param name="applicantId">The id of the applicant.</param>
		/// <returns>One applicant, if present.</returns>
		public Applicant GetApplicantAddressById(int applicantId) => DBApplicantToServiceObject(DBHelper.GetApplicantAddressById(applicantId));

		/// <summary>
		/// Retrieves the applicant with a given id.
		/// </summary>
		/// <param name="applicantId">The id of the applicant.</param>
		/// <returns>One applicant, if present.</returns>
		public Applicant GetApplicantById(int applicantId) => DBApplicantToServiceObject(DBHelper.GetApplicantById(applicantId));

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
		public ApplicationDocument GetApplicationDocumentById(int applicationDocumentId) => DBApplicationDocumentsToServiceObject(DBHelper.GetApplicationDocumentById(applicationDocumentId));

		public Applicant GetApplicantAddressByApplicationId(int applicationId) => DBApplicantToServiceObject(DBHelper.GetApplicantAddressById(DBHelper.GetApplicantByApplicationId(applicationId).ApplicantId));

		/// <summary>
		/// Retrieves the applicant that submitted the given application.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>The applicant that submitted the given application.</returns>
		public Applicant GetApplicantByApplicationId(int applicationId) => DBApplicantToServiceObject(DBHelper.GetApplicantByApplicationId(applicationId));

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
		public List<Application> GetApplicationsByPositionId(int positionId) => DBHelper.GetApplicationsByPositionId(positionId).Select(DBApplicationToServiceObject).ToList();

		/// <summary>
		/// Retrieves all applications that have been submitted by the given applicant.
		/// </summary>
		/// <param name="applicantId">The id of the applicant.</param>
		/// <returns>All applications that have been submitted by the given applicant.</returns>
		public List<Application> GetApplicationsByApplicantId(int applicantId) => DBHelper.GetApplicationsByApplicantId(applicantId).Select(DBApplicationToServiceObject).ToList();

		/// <summary>
		/// Retrieves all application documents attached to the given application.
		/// </summary>
		/// <param name="applicationId">The id of the application, containing multiple application documents.</param>
		/// <returns>All application documents attached to the given application.</returns>
		public List<ApplicationDocument> GetApplicationDocumentsByApplicationId(int applicationId) => DBHelper.GetApplicationDocumentsByApplicationId(applicationId).Select(DBApplicationDocumentsToServiceObject).ToList();

		/// <summary>
		/// Gets full application list with applicant infor and documents for each.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public List<Application> GetFullApplicationInfoByPositionId(int position) => SetupFullApplicationInfo(DBHelper.GetFullApplicationInfoByPositionId(position));

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
		public List<Position> GetPositionsByOrganizationId(int organizationId) => DBPositionsAndTagsToList(DBHelper.GetPositionsByOrganizationId(organizationId));

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

		/// <summary>
		/// get the default status ID from staffingsettings
		/// </summary>
		/// <param name="orgId"></param>
		/// <returns></returns>
		async public Task<List<int>> GetStaffingDefaultStatus(int orgId) => await DBHelper.GetStaffingDefaultStatus(orgId);

		#endregion GetMethods

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

		/// <summary>
		/// update default status
		/// </summary>
		/// <param name="organizationId"></param>
		/// <param name="positionStatusId"></param>
		public void UpdateDefaultPositionStatus(int organizationId, int positionStatusId) => DBHelper.UpdateStaffingSettings(organizationId, positionStatusId);

		#endregion UpdateMethods

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

		/// <summary>
		/// delete a specific employment type
		/// </summary>
		/// <param name="employmentTypeId"></param>
		public void DeleteEmploymentType(int employmentTypeId) => DBHelper.DeleteEmploymentType(employmentTypeId);

		/// <summary>
		/// delete a specific position level
		/// </summary>
		/// <param name="positionLevelId"></param>
		public void DeletePositionLevel(int positionLevelId) => DBHelper.DeletePositionLevel(positionLevelId);

		/// <summary>
		/// delete a specific position status
		/// </summary>
		/// <param name="positionStatusId"></param>
		public void DeletePositionStatus(int positionStatusId) => DBHelper.DeletePositionStatus(positionStatusId);

		#endregion DeleteMethods

		//////////////////////////////////////////
		// DB LAYER to SERVICE LAYER CONVERSION //
		//////////////////////////////////////////

		#region DB to Service Conversions

		public static Applicant DBApplicantToServiceObject(dynamic applicant)
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

		public static Application DBApplicationToServiceObject(dynamic application)
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
				ApplicationDocuments = DBApplicationDocumentsToServiceObject(application.ApplicationDocuments).ToList(),
				ApplicationId = application.ApplicationId,
				ApplicationModifiedUtc = application.ApplicationModifiedUtc,
				ApplicationStatus = application.ApplicationStatusId,
				PositionId = application.PositionId
			};
		}

		public static Applicant DBApplicantToServiceObject(ApplicantAddressDBEntity applicant)
		{
			if (applicant == null)
			{
				throw new ArgumentNullException(nameof(applicant), nameof(applicant) + " must not be null.");
			}

			return new Applicant
			{
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

		public static Application DBApplicationToServiceObject(ApplicationDBEntity application)
		{
			if (application == null)
			{
				throw new ArgumentNullException(nameof(application), nameof(application) + " must not be null.");
			}

			return new Application
			{
				ApplicantId = application.ApplicantId,
				Notes = application.Notes,
				ApplicationCreatedUtc = application.ApplicationCreatedUtc,
				ApplicationId = application.ApplicationId,
				ApplicationModifiedUtc = application.ApplicationModifiedUtc,
				ApplicationStatus = application.ApplicationStatusId,
				PositionId = application.PositionId
			};
		}

		public static ApplicationDocument DBApplicationDocumentsToServiceObject(dynamic applicationDocument)
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
		/// Builds an Application list which includes all relevant information for earch application (gets the applicant and documents too)
		/// </summary>
		/// <param name="applicationAndApplicantInfo"></param>
		/// <returns></returns>
		public static List<Application> SetupFullApplicationInfo(dynamic applicationAndApplicantInfo)
		{
			var applications = applicationAndApplicantInfo.applications;
			var results = new List<Application>();
			foreach (var application in applications)
			{
				var Applicant = new Applicant();
				foreach (var applicant in applicationAndApplicantInfo.applicants)
				{
					if (applicant.ApplicantId == application.ApplicantId)
					{
						Applicant.ApplicantId = applicant.ApplicantId;
						Applicant.FirstName = applicant.FirstName;
						Applicant.LastName = applicant.LastName;
						Applicant.City = applicant.City;
						Applicant.Country = applicant.CountryCode;
						Applicant.State = applicant.StateId;
						Applicant.Email = applicant.Email;
						Applicant.PhoneNumber = applicant.PhoneNumber;
						Applicant.Notes = applicant.Notes;
					}
				}
				var ApplicationDocuments = new List<ApplicationDocument>();
				foreach (var document in applicationAndApplicantInfo.documents)
				{
					if (document.ApplicationId == application.ApplicationId)
					{
						ApplicationDocuments.Add(new ApplicationDocument()
						{
							ApplicationId = document.ApplicationId,
							ApplicationDocumentId = document.ApplicationDocumentId,
							DocumentLink = document.DocumentLink,
							DocumentName = document.DocumentName
						});
					}
				}
				results.Add(new Application()
				{
					ApplicantId = application.ApplicantId,
					ApplicationId = application.ApplicationId,
					ApplicationStatus = application.ApplicationStatusId,
					ApplicationModifiedUtc = application.ApplicationModifiedUtc,
					Notes = application.Notes,
					Applicant = Applicant,
					ApplicationDocuments = ApplicationDocuments
				});
			}
			return results;
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
				OrganizationId = position.position.OrganizationId,
				CustomerId = position.position.CustomerId,
				AddressId = position.position.AddressId,
				PositionStatusId = position.position.PositionStatusId,
				PositionTitle = position.position.PositionTitle,
				DurationMonths = position.position.DurationMonths,
				EmploymentTypeId = position.position.EmploymentTypeId,
				PositionCount = position.position.PositionCount,
				RequiredSkills = position.position.RequiredSkills,
				PositionLevelId = position.position.PositionLevelId,
				PositionId = position.position.PositionId,
				PositionCreatedUtc = position.position.PositionCreatedUtc,
				PositionModifiedUtc = position.position.PositionModifiedUtc,
				StartDate = position.position.StartDate,
				BillingRateFrequency = position.position.BillingRateFrequency,
				BillingRateAmount = position.position.BillingRateAmount,
				JobResponsibilities = position.position.JobResponsibilities,
				DesiredSkills = position.position.DesiredSkills,
				HiringManager = position.position.HiringManager,
				TeamName = position.position.TeamName,

				Address = new Address
				{
					Address1 = position.position.Address1,
					Address2 = position.position.Address2,
					City = position.position.City,
					StateId = position.position.StateId,
					CountryCode = position.position.CountryCode,
					PostalCode = position.position.PostalCodes,
					StateName = position.position.StateName,
					CountryName = position.position.CountryName
				},

				Tags = DBDynamicToServiceObject(position.tags),

				EmploymentTypeName = position.position.EmploymentTypeName,
				PositionLevelName = position.position.PositionLevelName,
				PositionStatusName = position.position.PositionStatusName
			};
		}

		/// <summary>
		/// Converts a DB Layer PositionDBEntity object to a service layer Position Object.
		/// </summary>
		/// <param name="positionsAndTags">expando object containing a list or positions and a list of all tags belonging to all positions</param>
		/// <returns>Returns a service layer Position Obejct. </returns>
		public static List<Position> DBPositionsAndTagsToList(dynamic positionsAndTags)
		{
			List<Position> positions = new List<Position>();
			foreach (dynamic position in positionsAndTags.positions)
			{
				Position newPosition = new Position
				{
					PositionId = position.PositionId,
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
						Address1 = position.Address1,
						Address2 = position.Address2,
						City = position.City,
						StateId = position.StateId,
						CountryCode = position.CountryCode,
						PostalCode = position.PostalCodes
					},

					Tags = new List<Tag>()
				};

				try
				{
					foreach (dynamic tag in positionsAndTags.tags[position.PositionId])
					{
						newPosition.Tags.Add(new Tag
						{
							TagId = tag.TagId,
							TagName = tag.TagName
						});
					}
				}
				catch (RuntimeBinderException)
				{
				}

				positions.Add(newPosition);
			}

			return positions;
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
		/// Converts a list of TagDBEntity DB layer objects to a list of Tag service layer object.
		/// </summary>
		/// <param name="tags"> The List of tag DB layer objects to be converted. </param>
		/// <returns> Returns a list of Tag service layer objects.  </returns>
		public static List<Tag> DBDynamicToServiceObject(List<dynamic> tags)
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

		#endregion DB to Service Conversions

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

		public static ApplicantAddressDBEntity ServiceObjectToDBEntity(Applicant applicant)
		{
			if (applicant == null)
			{
				throw new ArgumentNullException(nameof(applicant), nameof(applicant) + " must not be null.");
			}

			return new ApplicantAddressDBEntity
			{
				ApplicantId = applicant.ApplicantId,
				Email = applicant.Email,
				FirstName = applicant.FirstName,
				LastName = applicant.LastName,
				Notes = applicant.Notes,
				OrganizationId = applicant.OrgId,
				PhoneNumber = applicant.PhoneNumber,
				Address1 = applicant.Address,
				Address2 = "",
				City = applicant.City,
				Country = applicant.Country,
				PostalCode = applicant.PostalCode,
				StateId = 1 //applicant.State
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
			obj.Position.PositionTitle = position.PositionTitle;
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

			if (position.Tags != null && position.Tags.Count != 0) obj.Tags = ServiceObjectToDBEntity(position.Tags);
			else obj.Tags = new List<dynamic>();

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

			List<dynamic> list = new List<dynamic>();
			foreach (Tag tag in tags)
			{
				dynamic obj = new ExpandoObject();
				obj.TagName = tag.TagName;
				obj.TagId = tag.TagId;
				obj.PositionId = tag.PositionId;
				list.Add(obj);
			}

			return list;
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