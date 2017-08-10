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

		/// <summary>
		/// Adds an applicant to the DB if there is not already another applicant with the same email.
		/// </summary>
		/// <param name="applicant">The applicant object to be added to the db.</param>
		/// <returns>The id of the created address and applicant</returns>
		public Tuple<int, int> CreateApplicant(ApplicantDBEntity applicant)
		{
			if (applicant == null)
			{
				throw new System.ArgumentException("applicant cannot be null or empty.");
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

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
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
				throw new System.ArgumentException("application cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicantId", application.ApplicantId);
			parameters.Add("@positionId", application.PositionId);
			parameters.Add("@applicationStatusId", application.ApplicationStatusId);
			parameters.Add("@notes", application.Notes);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
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
				throw new System.ArgumentException("application document cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationId", applicationDocument.ApplicationId);
			parameters.Add("@documentLink", applicationDocument.DocumentLink);
			parameters.Add("@documentName", applicationDocument.DocumentName);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
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
				throw new System.ArgumentException("Position cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", position.OrganizationId);
			parameters.Add("@AddressId", position.AddressId);
			parameters.Add("@StartDate", position.StartDate);
			parameters.Add("@PositionStatus", position.PositionStatus);
			parameters.Add("@PositionTitle", position.PositionTitle);
			parameters.Add("@BillingRateFrequency", position.BillingRateFrequency);
			parameters.Add("@BillingRateAmount", position.BillingRateAmount);
			parameters.Add("@DurationMonths", position.DurationMonths);
			parameters.Add("@EmploymentType", position.EmploymentType);
			parameters.Add("@PositionCount", position.PositionCount);
			parameters.Add("@RequiredSkills", position.RequiredSkills);
			parameters.Add("@JobResponsiblities", position.JobResponsibilities);
			parameters.Add("@DesiredSkills", position.DesiredSkills);
			parameters.Add("@PositionLevel", position.PositionLevel);
			parameters.Add("@HiringManager", position.HiringManager);
			parameters.Add("@TeamName", position.TeamName);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<int>("[StaffingManager].[CreatePosition]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Adds an Tag to the DB if there is not already another tag with the same name.
		/// </summary>
		/// <param name="name">The name of the tag to be added to the db.</param>
		/// <returns>The id of the created Tag or -1 if the tag name is already in use.</returns>
		public int CreateTag(string name)
		{
			if (name == null)
			{
				throw new System.ArgumentException("Name cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@TagName", name);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				// default -1
				connection.Execute("[StaffingManager].[Tag]", parameters, commandType: CommandType.StoredProcedure);
			}

			return parameters.Get<int>("@returnValue");
		}


		////////////////////////////
		/*          READ          */
		////////////////////////////

		/// <summary>
		/// Retrieves the Position with a given id.
		/// </summary>
		/// <param name="positionId">The id of the position.</param>
		/// <returns>One Position.</returns>
		public dynamic GetPositionByPositionId(int positionId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@PositionId", positionId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<dynamic>("[StaffingManager].[GetPositionByPositionId]", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
			}
		}

		/// <summary>
		/// Retrieves the Positions from a given orgnizatin.
		/// </summary>
		/// <param name="organizationId">The id of the organization.</param>
		/// <returns>One Position.</returns>
		public IEnumerable<dynamic> GetPositionsByOrganizationId(int organizationId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@OrganizationId", organizationId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<dynamic>("[StaffingManager].[GetPositionsByOrganizationId]", parameters, commandType: CommandType.StoredProcedure);
			}
		}


		////////////////////////////
		/*         UPDATE         */
		////////////////////////////

		/// <summary>
		/// Updates the given applicant if there is not already another applicant with the same email.
		/// </summary>
		/// <param name="applicant">The applicant object to be updated.</param>
		/// <returns>The number of rows updated.</returns>
		public int UpdateApplicant(ApplicantDBEntity applicant)
		{
			if (applicant == null)
			{
				throw new System.ArgumentException("applicant cannot be null or empty.");
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

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
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
				throw new System.ArgumentException("application cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationId", application.ApplicationId);
			parameters.Add("@applicationStatusId", application.ApplicationStatusId);
			parameters.Add("@notes", application.Notes);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
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
				throw new System.ArgumentException("application document cannot be null or empty.");
			}

			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@applicationDocumentId", applicationDocument.ApplicationDocumentId);
			parameters.Add("@documentLink", applicationDocument.DocumentLink);
			parameters.Add("@documentName", applicationDocument.DocumentName);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
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
				throw new System.ArgumentException("Position cannot be null or empty.");
			}
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@PositionId", position.PositionId);
			parameters.Add("@OrganizationId", position.OrganizationId);
			parameters.Add("@AddressId", position.AddressId);
			parameters.Add("@StartDate", position.StartDate);
			parameters.Add("@PositionStatus", position.PositionStatus);
			parameters.Add("@PositionTitle", position.PositionTitle);
			parameters.Add("@BillingRateFrequency", position.BillingRateFrequency);
			parameters.Add("@BillingRateAmount", position.BillingRateAmount);
			parameters.Add("@DurationMonths", position.DurationMonths);
			parameters.Add("@EmploymentType", position.EmploymentType);
			parameters.Add("@PositionCount", position.PositionCount);
			parameters.Add("@RequiredSkills", position.RequiredSkills);
			parameters.Add("@JobResponsibilities", position.JobResponsibilities);
			parameters.Add("@DesiredSkills", position.DesiredSkills);
			parameters.Add("@PositionLevel", position.PositionLevel);
			parameters.Add("@HiringManager", position.HiringManager);
			parameters.Add("@TeamName", position.TeamName);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Execute("[StaffingManager].[UpdatePosition]", parameters, commandType: CommandType.StoredProcedure);
			}
		}


		////////////////////////////
		/*         DELETE         */
		////////////////////////////

		/// <summary>
		/// Deletes a tag from the database
		/// </summary>
		/// <param name="tagId">Parameter @TagId. .</param>
		public void DeleteTag(int tagId)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@TagId", tagId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeleteTag]", parameters, commandType: CommandType.StoredProcedure);
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
			parameters.Add("@TagId", tagId);
			parameters.Add("@PositionId", positionId);

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
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

			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				connection.Execute("[StaffingManager].[DeletePosition]", parameters, commandType: CommandType.StoredProcedure);
			}
		}
	}
}
