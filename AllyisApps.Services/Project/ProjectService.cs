//------------------------------------------------------------------------------
// <copyright file="ProjectService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using AllyisApps.DBModel.Crm;

namespace AllyisApps.Services
{
	/// <summary>
	/// The Poject Service.
	/// </summary>
	public partial class Service : BaseService
	{
		/// <summary>
		/// Gets a list of <see cref="ProjectInfo"/>'s for a customer.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns>List of ProjectInfo's.</returns>
		public IEnumerable<ProjectInfo> GetProjectsByCustomer(int customerId)
		{
			if (customerId <= 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be 0 or negative.");
			}

			IEnumerable<ProjectDBEntity> dbeList = DBHelper.GetProjectsByCustomer(customerId);
			List<ProjectInfo> list = new List<ProjectInfo>();
			foreach (ProjectDBEntity dbe in dbeList)
			{
				if (dbe != null)
				{
					list.Add(InitializeProjectInfo(dbe));
				}
			}

			return list;
		}

		/// <summary>
		/// Creates a new project.
		/// </summary>
		/// <param name="newProject">ProjectInfo with project information.</param>
		/// <returns>Project Id.</returns>
		public int CreateProject(ProjectInfo newProject)
		{
			#region Validation

			if (newProject.CustomerId <= 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(newProject.Name))
			{
				throw new ArgumentNullException("name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(newProject.Type))
			{
				throw new ArgumentNullException("type", "Type must have a value.");
			}

			if (string.IsNullOrEmpty(newProject.ProjectOrgId))
			{
				throw new ArgumentNullException("projectOrgId", "Project must have an ID");
			}

			//if (newProject.StartingDate == null)
			//{
			//	throw new ArgumentNullException("start", "Project must have a start time");
			//}

			//if (newProject.EndingDate == null)
			//{
			//	throw new ArgumentNullException("end", "Project must have an end time");
			//}

			if (newProject.StartingDate.HasValue && newProject.EndingDate.HasValue && DateTime.Compare(newProject.StartingDate.Value, newProject.EndingDate.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			return DBHelper.CreateProject(GetDBEntityFromProjectInfo(newProject));
		}

		/// <summary>
		/// Updates a project's properties.
		/// </summary>
		/// <param name="project">ProjectInfo with updated properties.</param>
		public void UpdateProject(ProjectInfo project)
		{
			#region Validation

			if (project.ProjectId <= 0)
			{
				throw new ArgumentOutOfRangeException("ProjectId", "Project Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(project.Name))
			{
				throw new ArgumentNullException("Name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(project.Type))
			{
				throw new ArgumentNullException("Type", "Type must have a value.");
			}

			//if (project.StartingDate == null)
			//{
			//	throw new ArgumentNullException("StartingDate", "Project must have a start time");
			//}

			//if (project.EndingDate == null)
			//{
			//	throw new ArgumentNullException("EndingDate", "Project must have an end time");
			//}

			if (project.StartingDate.HasValue && project.EndingDate.HasValue && DateTime.Compare(project.StartingDate.Value, project.EndingDate.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			#endregion Validation

			if (this.Can(Actions.CoreAction.EditProject))
			{
				DBHelper.UpdateProject(GetDBEntityFromProjectInfo(project));
			}
		}

		/// <summary>
		/// Updates a project's properties and user list.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="name">Project name.</param>
		/// <param name="orgId">Project org id.</param>
		/// <param name="type">Project type.</param>
		/// <param name="start">Starting date. <see cref="DateTime"/></param>
		/// <param name="end">Ending date. <see cref="DateTime"/></param>
		/// <param name="userIDs">Updated on-project user list.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool UpdateProjectAndUsers(int projectId, string name, string orgId, string type, DateTime? start, DateTime? end, IEnumerable<int> userIDs)
		{
			#region Validation

			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name", "Project name must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrWhiteSpace(orgId))
			{
				throw new ArgumentNullException("orgId", "Project Org Id must have a value and cannot be whitespace.");
			}

			if (string.IsNullOrEmpty(type))
			{
				throw new ArgumentNullException("type", "Type must have a value.");
			}

			//if (start == null)
			//{
			//	throw new ArgumentNullException("start", "Project must have a start time");
			//}

			//if (end == null)
			//{
			//	throw new ArgumentNullException("end", "Project must have an end time");
			//}

			if (start.HasValue && end.HasValue && DateTime.Compare(start.Value, end.Value) > 0)
			{
				throw new ArgumentException("Project cannot end before it starts.");
			}

			if (userIDs == null)
			{
				userIDs = new List<int>();
			}

			#endregion Validation

			if (this.Can(Actions.CoreAction.EditProject))
			{
				DBHelper.UpdateProjectAndUsers(projectId, name, orgId, type, start, end, userIDs);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Deletes a project.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>Returns false if authorization fails.</returns>
		public bool DeleteProject(int projectId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (this.Can(Actions.CoreAction.EditProject))
			{
				DBHelper.DeleteProject(projectId);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a project user.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="userId">User Id.</param>
		public void CreateProjectUser(int projectId, int userId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			DBHelper.CreateProjectUser(projectId, userId);
		}

		/// <summary>
		/// Updates a project user.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="userId">User Id.</param>
		/// <param name="isActive">Active status to update to.</param>
		/// <returns>Number of rows updated.</returns>
		public int UpdateProjectUser(int projectId, int userId, bool isActive)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			return DBHelper.UpdateProjectUser(projectId, userId, isActive ? 1 : 0);
		}

		/// <summary>
		/// Deletes a project user.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <param name="userId">User Id.</param>
		/// <returns>Bool indication of success.</returns>
		public bool DeleteProjectUser(int projectId, int userId)
		{
			if (projectId <= 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be 0 or negative.");
			}

			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			return DBHelper.DeleteProjectUser(projectId, userId) == 1;
		}

		/// <summary>
		/// Gets all the projects a user can use in the chosen organization.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="isActive">True (default) to only return active projects, false to include all projects, active or not.</param>
		/// <returns>A list of all the projects a user can access in an organization.</returns>
		public IEnumerable<CompleteProjectInfo> GetProjectsByUserAndOrganization(int userId, bool isActive = true)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User Id cannot be 0 or negative.");
			}

			return DBHelper.GetProjectsByUserAndOrganization(userId, UserContext.ChosenOrganizationId, isActive ? 1 : 0).Select(c => InitializeCompleteProjectInfo(c));
		}

		/// <summary>
		/// Gets a <see cref="CompleteProjectInfo"/>.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>CompleteProjectInfo instance.</returns>
		public CompleteProjectInfo GetProject(int projectId)
		{
			if (projectId < 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be negative.");
			}

			return InitializeCompleteProjectInfo(DBHelper.GetProjectById(projectId));
		}

		/// <summary>
		/// Gets a <see cref="CompleteProjectInfo"/>, with the IsProjectUser field filled out for the
		/// current user.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns>CompleteProjectInfo instance.</returns>
		public CompleteProjectInfo GetProjectAsUser(int projectId)
		{
			if (projectId < 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be negative.");
			}

			return InitializeCompleteProjectInfo(DBHelper.GetProjectByIdAndUser(projectId, UserContext.UserId));
		}

		/// <summary>
		/// Gets a CompleteProjectInfo for the given project, a list of UserInfos for the project's assigned
		/// users, and a list of SubscriptionUserInfos for all users in the current subscription.
		/// </summary>
		/// <param name="projectId">Project Id.</param>
		/// <returns></returns>
		public Tuple<CompleteProjectInfo, List<UserInfo>, List<SubscriptionUserInfo>> GetProjectEditInfo(int projectId)
		{
			if (projectId < 0)
			{
				throw new ArgumentOutOfRangeException("projectId", "Project Id cannot be negative.");
			}

			var spResults = DBHelper.GetProjectEditInfo(projectId, UserContext.ChosenSubscriptionId);
			return Tuple.Create(
				InitializeCompleteProjectInfo(spResults.Item1),
				spResults.Item2.Select(udb => InitializeUserInfo(udb)).ToList(),
				spResults.Item3.Select(sudb => InitializeSubscriptionUserInfo(sudb)).ToList());
		}

		/// <summary>
		/// Gets the next logical project id for the given customer and a list of SubscriptionUserInfos for
		/// all useres in the current subscription.
		/// </summary>
		/// <param name="customerId">Customer Id.</param>
		/// <returns></returns>
		public Tuple<string, List<SubscriptionUserInfo>> GetNextProjectIdAndSubUsers(int customerId)
		{
			if (customerId < 0)
			{
				throw new ArgumentOutOfRangeException("customerId", "Customer Id cannot be negative.");
			}

			var spResults = DBHelper.GetNextProjectIdAndSubUsers(customerId, UserContext.ChosenSubscriptionId);
			return Tuple.Create(
				spResults.Item1 == null ? "0000000000000000" : new string(IncrementAlphanumericCharArray(spResults.Item1.ToCharArray())),
				spResults.Item2.Select(sudb => InitializeSubscriptionUserInfo(sudb)).ToList());
		}

		public IEnumerable<ProjectInfo> GetAllProjectsForOrganization(int orgId)
		{
			var result = new List<ProjectInfo>();
			foreach (var customer in this.GetCustomerList(orgId))
			{
				result.AddRange(this.GetProjectsByCustomer(customer.CustomerId));
			}
			return result;
		}

		#region Info-DBEntity Conversions
		/// <summary>
		/// Translates a <see cref="ProjectDBEntity"/> into a <see cref="ProjectInfo"/>.
		/// </summary>
		/// <param name="project">ProjectDBEntity instance.</param>
		/// <returns>ProjectInfo instance.</returns>
		public static ProjectInfo InitializeProjectInfo(ProjectDBEntity project)
		{
			if (project == null)
			{
				return null;
			}

			return new ProjectInfo
			{
				CustomerId = project.CustomerId,
				EndingDate = project.EndingDate,
				Name = project.Name,
				OrganizationId = project.OrganizationId,
				ProjectId = project.ProjectId,
				ProjectOrgId = project.ProjectOrgId,
				StartingDate = project.StartingDate,
				Type = project.Type
			};
		}

		/// <summary>
		/// Translates a <see cref="ProjectInfo"/> into a <see cref="ProjectDBEntity"/>.
		/// </summary>
		/// <param name="project">ProjectInfo instance.</param>
		/// <returns>ProjectDBEntity instance.</returns>
		public static ProjectDBEntity GetDBEntityFromProjectInfo(ProjectInfo project)
		{
			if (project == null)
			{
				return null;
			}

			return new ProjectDBEntity
			{
				CustomerId = project.CustomerId,
				EndingDate = project.EndingDate,
				Name = project.Name,
				OrganizationId = project.OrganizationId,
				ProjectId = project.ProjectId,
				ProjectOrgId = project.ProjectOrgId,
				StartingDate = project.StartingDate,
				Type = project.Type
			};
		}

		/// <summary>
		/// Translates a <see cref="CompleteProjectDBEntity"/> into a <see cref="CompleteProjectInfo"/>.
		/// </summary>
		/// <param name="completeProject">CompleteProjectDBEntity instance.</param>
		/// <returns>CompleteProjectInfo instance.</returns>
		public static CompleteProjectInfo InitializeCompleteProjectInfo(CompleteProjectDBEntity completeProject)
		{
			if (completeProject == null)
			{
				return null;
			}

			return new CompleteProjectInfo
			{
				CreatedUTC = completeProject.CreatedUTC,
				CustomerId = completeProject.CustomerId,
				CustomerName = completeProject.CustomerName,
				CustomerOrgId = completeProject.CustomerOrgId,
				EndDate = completeProject.EndDate,
				IsActive = completeProject.IsActive,
				IsCustomerActive = completeProject.IsCustomerActive,
				IsUserActive = completeProject.IsUserActive,
				OrganizationId = completeProject.OrganizationId,
				OrganizationName = completeProject.OrganizationName,
				OrgRoleId = completeProject.OrgRoleId,
				PriceType = completeProject.PriceType,
				ProjectId = completeProject.ProjectId,
				ProjectName = completeProject.ProjectName,
				StartDate = completeProject.StartDate,
				ProjectOrgId = completeProject.ProjectOrgId,
				IsProjectUser = completeProject.IsProjectUser
			};
		}
		#endregion
	}
}
