//------------------------------------------------------------------------------
// <copyright file="DBHelper.StaffingManager.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using AllyisApps.DBModel.Crm;
using AllyisApps.DBModel.Lookup;
using AllyisApps.DBModel.StaffingManager;
using Dapper;

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
		public int CreateApplicant(dynamic applicant)
		{
			if (applicant == null)
			{
				throw new ArgumentException("applicant cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@email", applicant.Email);
			parameters.Add("@firstName", applicant.FirstName);
			parameters.Add("@lastName", applicant.LastName);
			parameters.Add("@address1", applicant.Address1);
			parameters.Add("@address2", applicant.Address2);
			parameters.Add("@city", applicant.City);
			parameters.Add("@stateId", applicant.StateId);
			parameters.Add("@postalCode", applicant.PostalCode);
			parameters.Add("@countryCode", "US"); // add real country code
			parameters.Add("@phoneNumber", applicant.PhoneNumber);
			parameters.Add("@notes", applicant.Notes);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
			return connection.Query<int>("[StaffingManager].[CreateApplicant]", parameters, commandType: CommandType.StoredProcedure).Single();
			}
		}

		/// <summary>
		/// Adds an application to the DB.
		/// </summary>
		/// <param name="application">The application object to be added to the db.</param>
		/// <returns>The id of the created application</returns>
		public int CreateApplication(dynamic application)
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
		public int CreateApplicationDocument(dynamic applicationDocument)
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
		/// <param name="obj">The account object to be created.</param>
		/// <returns>The id of the created position</returns>
		public int SetupPosition(dynamic obj)
		{
			if (obj == null)
			{
				throw new ArgumentException("Position cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", obj.Position.OrganizationId);
			parameters.Add("@customerId", obj.Position.CustomerId);
			parameters.Add("@startDate", obj.Position.StartDate);
			parameters.Add("@positionStatus", obj.Position.PositionStatusId);
			parameters.Add("@positionTitle", obj.Position.PositionTitle);
			parameters.Add("@billingRateFrequency", obj.Position.BillingRateFrequency);
			parameters.Add("@billingRateAmount", obj.Position.BillingRateAmount);
			parameters.Add("@durationMonths", obj.Position.DurationMonths);
			parameters.Add("@employmentType", obj.Position.EmploymentTypeId);
			parameters.Add("@positionCount", obj.Position.PositionCount);
			parameters.Add("@requiredSkills", obj.Position.RequiredSkills);
			parameters.Add("@jobResponsibilities", obj.Position.JobResponsibilities);
			parameters.Add("@desiredSkills", obj.Position.DesiredSkills);
			parameters.Add("@positionLevel", obj.Position.PositionLevelId);
			parameters.Add("@hiringManager", obj.Position.HiringManager);
			parameters.Add("@teamName", obj.Position.TeamName);

			parameters.Add("@address1", obj.Address.Address1);
			parameters.Add("@address2", obj.Address.Address2);
			parameters.Add("@city", obj.Address.City);
			parameters.Add("@stateId", obj.Address.StateId);
			parameters.Add("@countryCode", obj.Address.CountryCode);
			parameters.Add("@postalCode", obj.Address.PostalCode);

			DataTable tagsTable = new DataTable();
			tagsTable.Columns.Add("TagName", typeof(string));
			if (obj.Tags != null && obj.Tags.Count != 0)
			{
				foreach (dynamic tag in obj.Tags) tagsTable.Rows.Add(tag.TagName);
				parameters.Add("@tags", tagsTable.AsTableValuedParameter("[Lookup].[TagTable]"));
			}
			else
			{
				tagsTable.Rows.Add("New");
				parameters.Add("@tags", tagsTable.AsTableValuedParameter("[Lookup].[TagTable]"));
			}

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Execute("[StaffingManager].[SetupPosition]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Creates a position level for the database
		/// </summary>
		/// <param name="positionLevel"> The name of the position Level to be created. </param>
		public void CreatePositionLevel(dynamic positionLevel)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", positionLevel.organizationId);
			parameters.Add("@positionLevelName", positionLevel.positionLevelName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[CreatePositionLevel]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Creates a position status for the database
		/// </summary>
		/// <param name="positionStatus">Parameter @positionStatusName.</param>
		public void CreatePositionStatus(dynamic positionStatus)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", positionStatus.organizationId);
			parameters.Add("@positionStatusName", positionStatus.positionStatusName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[CreatePositionStatus]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Creates a employment type for the database
		/// </summary>
		/// <param name="employmentType">Parameter @employmentTypeName.</param>
		public void CreateEmploymentType(dynamic employmentType)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", employmentType.organizationId);
			parameters.Add("@employmentTypeName", employmentType.employmentTypeName);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[CreateEmploymentType]", parameters, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Adds an Tag to the DB if there is NOT already another tag with the same name.
		/// </summary>
		/// <param name="tagName">The name of the tag to be added to the db.</param>
		/// <param name="positionId">The position the tag will be added to .</param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use.</returns>
		public int CreateTag(string tagName, int positionId)
		{
			if (tagName == null)
			{
				throw new ArgumentException("Name cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@tagName", tagName);
			parameters.Add("@positionId", positionId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default -1
				connection.Execute("[StaffingManager].[SetupTag]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@returnValue");
		}

		/// <summary>
		/// Adds a PositionTag to the DB when there IS ALREADY another tag with the same name.
		/// </summary>
		/// <param name="tagId">The ID of the tag to be added to the db.</param>
		/// <param name="positionId">The ID of the Position to be added to the db.</param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use.</returns>
		public void AssignTag(int tagId, int positionId)
		{
			if (positionId == 0)
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

		/// <summary>
		/// Adds an organizations staffing settings.
		/// </summary>
		/// <param name="orgId">org ID thats getting a new settings object</param>
		/// <returns>Creates an orgs staffing object</returns>
		public void CreateStaffingSettings(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default -1
				connection.Execute("[StaffingManager].[CreateStaffingSettings]", parameters, commandType: CommandType.StoredProcedure);
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
		public dynamic GetApplicationById(int applicationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationId", applicationId);

			dynamic applicationAndDocs = new ExpandoObject();
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = connection.QueryMultiple("[StaffingManager].[GetApplicationAndDocumentsById]", parameters, commandType: CommandType.StoredProcedure);
				applicationAndDocs.application = results.Read<ApplicationDBEntity>().Single();
				applicationAndDocs.applicationDocuments = results.Read<ApplicationDocumentDBEntity>().ToList();
			}
			return applicationAndDocs;
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
		public IEnumerable<ApplicationDBEntity> GetApplicationsByPositionId(int positionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionId", positionId);

			dynamic applicationsAndDocuments = new ExpandoObject();
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = connection.Query<ApplicationDBEntity>("[StaffingManager].[GetApplicationsByPositionId]", parameters, commandType: CommandType.StoredProcedure);
			}
			return applicationsAndDocuments;
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
		/// Retrieves the list of applicants.
		/// </summary>
		/// <param name="orgId"></param>
		/// <returns>All the applicants in a subscription.</returns>
		public List<ApplicantDBEntity> GetApplicantsBySubscriptionId(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@orgId", orgId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<ApplicantDBEntity>("[StaffingManager].[GetApplicantsByOrgId]", parameters, commandType: CommandType.StoredProcedure).ToList();
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
		public dynamic GetPositionById(int positionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@positionId", positionId);

			dynamic positionAndTags = new ExpandoObject();
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = connection.Query<dynamic>("[StaffingManager].[GetPosition]", parameters, commandType: CommandType.StoredProcedure).Single();
				positionAndTags.position = results.Read<dynamic>().Single();
				positionAndTags.tags = results.Read<dynamic>().ToList();
			}
			return positionAndTags;
		}

		/// <summary>
		/// Retrieves the Positions from a given orgnizatin.
		/// </summary>
		/// <param name="organizationId">The id of the organization.</param>
		/// <returns>One Position.</returns>
		public dynamic GetPositionsByOrganizationId(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", organizationId);

			dynamic positionsAndTags = new ExpandoObject();
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				var results = connection.QueryMultiple("[StaffingManager].[GetPositionsByOrganizationId]", parameters, commandType: CommandType.StoredProcedure);
				positionsAndTags.positions = results.Read<dynamic>().ToList();
				//positionsAndTags.tags = results.Read<dynamic>().ToDictionary(t => t.PositionId, t => t);
			}
			return positionsAndTags;
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
				return connection.Query<PositionStatusDBEntity>("[StaffingManager].[GetPositionStatusByOrganizationId]", parameters, commandType: CommandType.StoredProcedure);
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

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <returns>.</returns>
		public Tuple<List<PositionDBEntity>, List<PositionTagDBEntity>, List<EmploymentTypeDBEntity>, List<PositionLevelDBEntity>, List<PositionStatusDBEntity>, List<CustomerDBEntity>>
			GetStaffingIndexPageInfo(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[StaffingManager].[GetStaffingIndexInfo]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return Tuple.Create(
					results.Read<PositionDBEntity>().ToList(),
					results.Read<PositionTagDBEntity>().ToList(),
					results.Read<EmploymentTypeDBEntity>().ToList(),
					results.Read<PositionLevelDBEntity>().ToList(),
					results.Read<PositionStatusDBEntity>().ToList(),
					results.Read<CustomerDBEntity>().ToList());
			}
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="orgId">Organization Id.</param>
		/// <param name="statuses">Organization Id.</param>
		/// <param name="types">Organization Id.</param>
		/// <param name="tags">Organization Id.</param>
		/// <returns>.</returns>
		public Tuple<List<PositionDBEntity>, List<PositionTagDBEntity>, List<EmploymentTypeDBEntity>, List<PositionLevelDBEntity>, List<PositionStatusDBEntity>, List<CustomerDBEntity>>
			GetStaffingIndexPageInfoFiltered(int orgId, List<string> statuses, List<string> types, List<string> tags = null)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);

			DataTable StatusesTable = new DataTable();
			StatusesTable.Columns.Add("StatusName", typeof(string));
			foreach (string status in statuses) StatusesTable.Rows.Add(status);

			DataTable TypesTable = new DataTable();
			TypesTable.Columns.Add("TypeName", typeof(string));
			foreach (string type in types) TypesTable.Rows.Add(type);

			DataTable TagTable = new DataTable();
			TagTable.Columns.Add("TagName", typeof(string));
			foreach (string tag in tags) TagTable.Rows.Add(tag);

			parameters.Add("@status", StatusesTable.AsTableValuedParameter("[StaffingManager].[StatusesTable]"));
			parameters.Add("@type", TypesTable.AsTableValuedParameter("[StaffingManager].[TypesTable]"));
			parameters.Add("@tags", TagTable.AsTableValuedParameter("[Lookup].[TagTable]"));
			
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				var results = connection.QueryMultiple(
					"[StaffingManager].[GetStaffingIndexInfoFiltered]",
					parameters,
					commandType: CommandType.StoredProcedure);

				return Tuple.Create(
					results.Read<PositionDBEntity>().ToList(),
					results.Read<PositionTagDBEntity>().ToList(),
					results.Read<EmploymentTypeDBEntity>().ToList(),
					results.Read<PositionLevelDBEntity>().ToList(),
					results.Read<PositionStatusDBEntity>().ToList(),
					results.Read<CustomerDBEntity>().ToList());
			}
		}
		
		/// <summary>
		/// Updates an organizations staffing settings.
		/// </summary>
		/// <param name="orgId">org ID thats getting a new setting </param>
		/// <returns>Creates an orgs staffing object</returns>
		public int GetStaffingDefaultStatus(int orgId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default -1
				var returnInt = 0;
				var result = connection.Query("[StaffingManager].[GetStaffingDefaultStatus]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
				if (result.DefaultPositionStatusId == null) return returnInt;
				else return result.DefaultPositionStatusId;

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
		public int UpdateApplicant(dynamic applicant)
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

		/// <summary>
		/// Updates an organizations staffing settings.
		/// </summary>
		/// <param name="orgId">org ID thats getting a new setting </param>
		/// <param name="positionStatusId">position status to be set as default</param>
		/// <returns>Creates an orgs staffing object</returns>
		public void UpdateStaffingSettings(int orgId, int positionStatusId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@organizationId", orgId);
			parameters.Add("@positionStatusId", positionStatusId);

			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				// default -1
				connection.Execute("[StaffingManager].[UpdateStaffingSettings]", parameters, commandType: CommandType.StoredProcedure);
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