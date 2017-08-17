//------------------------------------------------------------------------------
// <copyright file="DBHelper.StaffingManager.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AllyisApps.DBModel.StaffingManager;
using Dapper;
using AllyisApps.DBModel.Lookup;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		////////////////////////////
		/*         CREATE         */
		////////////////////////////
		#region Create Methods
		/// <summary>
		/// Adds an applicant to the DB if there is not already another applicant with the same email.
		/// </summary>
		/// <param name="applicant">The applicant object to be added to the db.</param>
		/// <returns>The id of the created address and applicant</returns>
		public Tuple<int, int> CreateApplicant(ApplicantDBEntity applicant)
		{
			if (applicant == null)
			{
				throw new ArgumentException("applicant cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@email", applicant.Email);
			parameters.Add("@firstName", applicant.FirstName);
			parameters.Add("@lastName", applicant.LastName);
			parameters.Add("@address", applicant.Address);
			parameters.Add("@city", applicant.City);
			parameters.Add("@state", applicant.State);
			parameters.Add("@country", applicant.Country);
			parameters.Add("@postalCode", applicant.PostalCode);
			parameters.Add("@phoneNumber", applicant.PhoneNumber);
			parameters.Add("@notes", applicant.Notes);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				dynamic results = connection.Query<dynamic>("[StaffingManager].[CreateApplicant]", parameters, commandType: CommandType.StoredProcedure).Single();
				return Tuple.Create(results.AddressId, results.ApplicantId);
			}
		}

		/// <summary>
		/// Adds an application to the DB.
		/// </summary>
		/// <param name="application">The application object to be added to the db.</param>
		/// <returns>The id of the created application</returns>
		public int CreateApplication(ApplicationDBEntity application)
		{
			if (application == null)
			{
				throw new ArgumentException("application cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicantId", application.ApplicantId);
			parameters.Add("@positionId", application.PositionId);
			parameters.Add("@applicationStatusId", application.ApplicationStatusId);
			parameters.Add("@notes", application.Notes);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<int>("[StaffingManager].[CreateApplication]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Adds an application document to the DB, which joins to an application.
		/// </summary>
		/// <param name="applicationDocument">The application document object to be added to the db.</param>
		/// <returns>The id of the created application document</returns>
		public int CreateApplicationDocument(ApplicationDocumentDBEntity applicationDocument)
		{
			if (applicationDocument == null)
			{
				throw new ArgumentException("application document cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationId", applicationDocument.ApplicationId);
			parameters.Add("@documentLink", applicationDocument.DocumentLink);
			parameters.Add("@documentName", applicationDocument.DocumentName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<int>("[StaffingManager].[CreateApplicationDocument]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}


		/// <summary>
		/// Creates a new position.
		/// </summary>
		/// <param name="position">The account object to be created.</param>
		/// <returns>The id of the created position</returns>
		public int CreatePosition(PositionDBEntity position)
		{
			if (position == null)
			{
				throw new ArgumentException("Position cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", position.OrganizationId);
			parameters.Add("@addressId", position.AddressId);
			parameters.Add("@startDate", position.StartDate);
			parameters.Add("@positionStatus", position.PositionStatusId);
			parameters.Add("@positionTitle", position.PositionTitle);
			parameters.Add("@billingRateFrequency", position.BillingRateFrequency);
			parameters.Add("@billingRateAmount", position.BillingRateAmount);
			parameters.Add("@durationMonths", position.DurationMonths);
			parameters.Add("@employmentType", position.EmploymentTypeId);
			parameters.Add("@positionCount", position.PositionCount);
			parameters.Add("@requiredSkills", position.RequiredSkills);
			parameters.Add("@jobResponsiblities", position.JobResponsibilities);
			parameters.Add("@desiredSkills", position.DesiredSkills);
			parameters.Add("@positionLevel", position.PositionLevelId);
			parameters.Add("@hiringManager", position.HiringManager);
			parameters.Add("@teamName", position.TeamName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<int>("[StaffingManager].[CreatePosition]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}



		/// <summary>
		/// Creates a position level for the database
		/// </summary>
		/// <param name="organizationId"> The ID of the organization this level belongs to. </param>
		/// <param name="positionLevelName"> The name of the position Level to be created. </param>
		public void CreatePositionLevel(int organizationId, string positionLevelName)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@positionLevelName", positionLevelName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[CreatePositionLevel]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Creates a position status for the database
		/// </summary>
		/// <param name="organizationId"> The ID of the organization this status belongs to. </param>
		/// <param name="positionStatusName">Parameter @positionStatusName.</param>
		public void CreatePositionStatus(int organizationId, string positionStatusName)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@positionStatusName", positionStatusName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[CreatePositionStatus]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Creates a employment type for the database
		/// </summary>
		/// <param name="organizationId"> The ID of the organization this type belongs to. </param>
		/// <param name="employmentTypeName">Parameter @employmentTypeName.</param>
		public void CreateEmploymentType(int organizationId, string employmentTypeName)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);
			parameters.Add("@employmentTypeName", employmentTypeName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[CreateEmploymentType]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds an Tag to the DB if there is not already another tag with the same name.
		/// </summary>
		/// <param name="name">The name of the tag to be added to the db.</param>
		/// <param name="positionId">The name of the tag to be added to the db.</param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use.</returns>
		public int CreateTag(string name, int positionId)
		{
			if (name == null)
			{
				throw new ArgumentException("Name cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@tagName", name);
			parameters.Add("@positionId", positionId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default -1
				connection.Execute("[StaffingManager].[SetupTag]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@returnValue");
		}

		/// <summary>
		/// Adds a PositionTag to the DB when there is already another tag with the same name.
		/// </summary>
		/// <param name="tagId">The name of the tag to be added to the db.</param>
		/// <param name="positionId">The name of the tag to be added to the db.</param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use.</returns>
		public void AssignTag(int tagId, int positionId)
		{
			if (tagId == 0)
			{
				throw new ArgumentException("tag ID cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@tagId", tagId);
			parameters.Add("@positionId", positionId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default -1
				connection.Execute("[StaffingManager].[CreatePositionTag]", parameters, commandType: CommandType.StoredProcedure);
			}

		}

		#endregion Create Methods

		////////////////////////////
		/*          READ          */
		////////////////////////////
		#region Get Methods

		/// <summary>
		/// Retrieves the application with a given id.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>One application, if present.</returns>
		public ApplicationDBEntity GetApplicationById(int applicationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationId", applicationId);

			ApplicationDBEntity application;
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				application = connection.Query<ApplicationDBEntity>("[StaffingManager].[GetApplicationById]", parameters, commandType: CommandType.StoredProcedure).Single();
				application.ApplicationDocuments = connection.Query<ApplicationDocumentDBEntity>("[StaffingManager].[GetApplicationDocumentsByApplicationId]", parameters, commandType: CommandType.StoredProcedure);
			}
			return application;
		}

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
		public IEnumerable<ApplicationDBEntity> GetApplicationsInfoByPositionId(int positionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionId", positionId);

			IEnumerable<ApplicationDBEntity> applications;
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				applications = connection.Query<ApplicationDBEntity>("[StaffingManager].[GetApplicationsByPositionId]", parameters, commandType: CommandType.StoredProcedure);
				foreach (ApplicationDBEntity application in applications)
				{
					application.ApplicationDocuments = connection.Query<ApplicationDocumentDBEntity>(
						"[StaffingManager].[GetApplicationDocumentsByApplicationId]",
						new { applicationId = application.ApplicationId },
						commandType: CommandType.StoredProcedure);

					application.Applicant = connection.Query<ApplicantDBEntity>(
						"[StaffingManager].[GetApplicantById]",
						new { applicantId = application.ApplicantId },
						commandType: CommandType.StoredProcedure).Single();
				}
			}
			return applications;
		}

		/// <summary>
		/// Retrieves all applications that have been submitted by the given applicant.
		/// </summary>
		/// <param name="applicantId">The id of the applicant.</param>
		/// <returns>All applications that have been submitted by the given applicant.</returns>
		public IEnumerable<ApplicationDBEntity> GetApplicationsByApplicantId(int applicantId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicantId", applicantId);

			IEnumerable<ApplicationDBEntity> applications;
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				applications = connection.Query<ApplicationDBEntity>("[StaffingManager].[GetApplicationsByApplicantId]", parameters, commandType: CommandType.StoredProcedure);
				foreach (ApplicationDBEntity application in applications)
				{
					application.ApplicationDocuments = connection.Query<ApplicationDocumentDBEntity>(
						"[StaffingManager].[GetApplicationDocumentsByApplicationId]",
						new { applicationId = application.ApplicationId },
						commandType: CommandType.StoredProcedure);
				}
			}
			return applications;
		}

		/// <summary>
		/// Retrieves the application document with a given id.
		/// </summary>
		/// <param name="applicationDocumentId">The id of the application document.</param>
		/// <returns>One application document, if present.</returns>
		public ApplicationDocumentDBEntity GetApplicationDocumentById(int applicationDocumentId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationDocumentId", applicationDocumentId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<ApplicationDocumentDBEntity>("[StaffingManager].[GetApplicationDocumentById]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Retrieves all application documents attached to the given application.
		/// </summary>
		/// <param name="applicationId">The id of the application, containing multiple application documents.</param>
		/// <returns>All application documents attached to the given application.</returns>
		public IEnumerable<ApplicationDocumentDBEntity> GetApplicationDocumentsByApplicationId(int applicationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationId", applicationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<ApplicationDocumentDBEntity>("[StaffingManager].[GetApplicationDocumentsByApplicationId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the applicant with a given id.
		/// </summary>
		/// <param name="applicantId">The id of the applicant.</param>
		/// <returns>One applicant, if present.</returns>
		public ApplicantDBEntity GetApplicantById(int applicantId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicantId", applicantId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<ApplicantDBEntity>("[StaffingManager].[GetApplicantById]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Retrieves the applicant that submitted the given application.
		/// </summary>
		/// <param name="applicationId">The id of the application.</param>
		/// <returns>The applicant that submitted the given application.</returns>
		public ApplicantDBEntity GetApplicantByApplicationId(int applicationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationId", applicationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<ApplicantDBEntity>("[StaffingManager].[GetApplicantByApplicationId]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Retrieves the Position with a given id.
		/// </summary>
		/// <param name="positionId">The id of the position.</param>
		/// <returns>One Position.</returns>
		public PositionDBEntity GetPositionById(int positionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionId", positionId);

			PositionDBEntity position;
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				position = connection.Query<PositionDBEntity>("[StaffingManager].[GetPosition]", parameters, commandType: CommandType.StoredProcedure).Single();
				position.Tags = connection.Query<TagDBEntity>("[StaffingManager].[GetTagsByPositionId]", parameters, commandType: CommandType.StoredProcedure).ToList<TagDBEntity>();
			}
			return position;
		}

		/// <summary>
		/// Retrieves the Positions from a given orgnizatin.
		/// </summary>
		/// <param name="organizationId">The id of the organization.</param>
		/// <returns>One Position.</returns>
		public IEnumerable<PositionDBEntity> GetPositionsByOrganizationId(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);

			IEnumerable<PositionDBEntity> positions;
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				positions = connection.Query<PositionDBEntity>("[StaffingManager].[GetPositionsByOrganizationId]", parameters, commandType: CommandType.StoredProcedure);
				foreach (PositionDBEntity position in positions)
				{
					position.Tags = connection.Query<TagDBEntity>(
						"[StaffingManager].[GetTagsByPositionId]",
						new { positionId = position.PositionId },
						commandType: CommandType.StoredProcedure).ToList<TagDBEntity>();
				}
			}
			return positions;
		}

		/// <summary>
		/// Gets a position level from the database
		/// </summary>
		/// <param name="positionLevelId">Parameter @positionLevelId.</param>
		public PositionLevelDBEntity GetPositionLevelById(int positionLevelId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionLevelId", positionLevelId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<PositionLevelDBEntity>("[StaffingManager].[GetPositionLevelById]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Gets a position levels by organization from the database
		/// </summary>
		/// <param name="organizationId">Parameter @organizationId.</param>
		public IEnumerable<PositionLevelDBEntity> GetPositionLevelsByOrganization(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<PositionLevelDBEntity>("[StaffingManager].[GetPositionLevelsByOrganization]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets a position status from the database
		/// </summary>
		/// <param name="positionStatusId">Parameter @positionStatusId.</param>
		public PositionStatusDBEntity GetPositionStatusById(int positionStatusId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionStatusId", positionStatusId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<PositionStatusDBEntity>("[StaffingManager].[GetPositionStatusById]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Gets position statuses by an organization from the database
		/// </summary>
		/// <param name="organizationId">Parameter @organizationId.</param>
		public IEnumerable<PositionStatusDBEntity> GetPositionStatusesByOrganization(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<PositionStatusDBEntity>("[StaffingManager].[GetPositionStatusById]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Gets an employment type from the database
		/// </summary>
		/// <param name="employmentTypeId">Parameter @employmentTypeId.</param>
		public EmploymentTypeDBEntity GetEmploymentTypeById(int employmentTypeId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@employmentTypeId", employmentTypeId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<EmploymentTypeDBEntity>("[StaffingManager].[GetEmploymentTypeById]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Get employment types by organization from the database
		/// </summary>
		/// <param name="organizationId">Parameter @organizationId.</param>
		public IEnumerable<EmploymentTypeDBEntity> GetEmploymentTypesByOrganization(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<EmploymentTypeDBEntity>("[StaffingManager].[GetEmploymentTypesByOrganization]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves the tags on a position.
		/// </summary>
		/// <param name="positionId">The id of the posiion.</param>
		/// <returns>One Position.</returns>
		public IEnumerable<TagDBEntity> GetTagsByPositionId(int positionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionId", positionId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<TagDBEntity>("[StaffingManager].[GetTagsByPositionId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves All tags
		/// </summary>
		/// <returns>All the tags</returns>
		public IEnumerable<TagDBEntity> GetTags()
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<TagDBEntity>("[StaffingManager].[GetTags]", commandType: CommandType.StoredProcedure);
			}
		}

		#endregion Get Methods

		////////////////////////////
		/*         UPDATE         */
		////////////////////////////
		#region Update Methods

		/// <summary>
		/// Updates the given applicant if there is not already another applicant with the same email.
		/// </summary>
		/// <param name="applicant">The applicant object to be updated.</param>
		/// <returns>The number of rows updated.</returns>
		public int UpdateApplicant(ApplicantDBEntity applicant)
		{
			if (applicant == null)
			{
				throw new ArgumentException("applicant cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicantId", applicant.ApplicantId);
			parameters.Add("@addressId", applicant.AddressId);
			parameters.Add("@email", applicant.Email);
			parameters.Add("@firstName", applicant.FirstName);
			parameters.Add("@lastName", applicant.LastName);
			parameters.Add("@address", applicant.Address);
			parameters.Add("@city", applicant.City);
			parameters.Add("@state", applicant.State);
			parameters.Add("@country", applicant.Country);
			parameters.Add("@postalCode", applicant.PostalCode);
			parameters.Add("@phoneNumber", applicant.PhoneNumber);
			parameters.Add("@notes", applicant.Notes);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Execute("[StaffingManager].[UpdateApplicant]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates the given application.
		/// </summary>
		/// <param name="application">The application object to be updated.</param>
		/// <returns>The number of rows updated.</returns>
		public int UpdateApplication(ApplicationDBEntity application)
		{
			if (application == null)
			{
				throw new ArgumentException("application cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationId", application.ApplicationId);
			parameters.Add("@applicationStatusId", application.ApplicationStatusId);
			parameters.Add("@notes", application.Notes);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Execute("[StaffingManager].[UpdateApplication]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates the given application document.
		/// </summary>
		/// <param name="applicationDocument">The application document object to be updated.</param>
		/// <returns>The number of rows updated.</returns>
		public int UpdateApplicationDocument(ApplicationDocumentDBEntity applicationDocument)
		{
			if (applicationDocument == null)
			{
				throw new ArgumentException("application document cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationDocumentId", applicationDocument.ApplicationDocumentId);
			parameters.Add("@documentLink", applicationDocument.DocumentLink);
			parameters.Add("@documentName", applicationDocument.DocumentName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Execute("[StaffingManager].[UpdateApplicationDocument]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Updates the position with the given id.
		/// </summary>
		/// <param name="position">The account object to be updated.</param>
		/// <returns>Returns the number of rows updated.</returns>
		public int UpdatePosition(dynamic position)
		{
			if (position == null)
			{
				throw new ArgumentException("Position cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionId", position.PositionId);
			parameters.Add("@organizationId", position.OrganizationId);
			parameters.Add("@addressId", position.AddressId);
			parameters.Add("@startDate", position.StartDate);
			parameters.Add("@positionStatus", position.PositionStatus);
			parameters.Add("@positionTitle", position.PositionTitle);
			parameters.Add("@billingRateFrequency", position.BillingRateFrequency);
			parameters.Add("@billingRateAmount", position.BillingRateAmount);
			parameters.Add("@durationMonths", position.DurationMonths);
			parameters.Add("@employmentType", position.EmploymentType);
			parameters.Add("@positionCount", position.PositionCount);
			parameters.Add("@requiredSkills", position.RequiredSkills);
			parameters.Add("@jobResponsibilities", position.JobResponsibilities);
			parameters.Add("@desiredSkills", position.DesiredSkills);
			parameters.Add("@positionLevel", position.PositionLevel);
			parameters.Add("@hiringManager", position.HiringManager);
			parameters.Add("@teamName", position.TeamName);
			parameters.Add("@address", position.Address);
			parameters.Add("@city", position.City);
			parameters.Add("@state", position.State);
			parameters.Add("@country", position.Country);
			parameters.Add("@postalCode ", position.PostalCode);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Execute("[StaffingManager].[UpdatePosition]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		#endregion Update Methods

		////////////////////////////
		/*         DELETE         */
		////////////////////////////
		#region Delete Methods
		/// <summary>
		/// Deletes an applicant from the database
		/// </summary>
		/// <param name="applicantId">The applicant to be deleted</param>
		public void DeleteApplicant(int applicantId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicantId", applicantId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeleteApplicant]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes an application from the database
		/// </summary>
		/// <param name="applicationId">The applicant to be deleted</param>
		public void DeleteApplication(int applicationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationId", applicationId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeleteApplication]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes an application document from the database
		/// </summary>
		/// <param name="applicationDocumentId">The applicant to be deleted</param>
		public void DeleteApplicationDocument(int applicationDocumentId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationDocumentId", applicationDocumentId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeleteApplicationDocument]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a tag from the database
		/// </summary>
		/// <param name="tagId">Parameter @TagId. .</param>
		public void DeleteTag(int tagId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@tagId", tagId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeleteTag]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a position level from the database
		/// </summary>
		/// <param name="positionLevelId">Parameter @positionLevelId.</param>
		public void DeletePositionLevel(int positionLevelId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionLevelId", positionLevelId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeletePositionLevel]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a position status from the database
		/// </summary>
		/// <param name="positionStatusId">Parameter @positionStatusId.</param>
		public void DeletePositionStatus(int positionStatusId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionStatusId", positionStatusId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeletePositionStatus]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a employment level from the database
		/// </summary>
		/// <param name="employmentTypeId">Parameter @employmentTypeId.</param>
		public void DeleteEmploymentType(int employmentTypeId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@employmentTypeId", employmentTypeId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeleteEmploymentType]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Deletes a position tag from the database; Doesnt delete the tag, just removes it from the position
		/// </summary>
		/// <param name="tagId">Parameter @TagId. .</param>
		/// <param name="positionId">Parameter @TagId. .</param>
		public void DeletePositionTag(int tagId, int positionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@tagId", tagId);
			parameters.Add("@positionId", positionId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeletePositionTag]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Removes a Position from the Database
		/// </summary>
		/// <param name="positionId">Parameter @PositionId. .</param>
		public void DeletePosition(int positionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionId", positionId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeletePosition]", parameters, commandType: CommandType.StoredProcedure);
			}
		}
		#endregion Delete Methods
	}
}
