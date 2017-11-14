//------------------------------------------------------------------------------
// <copyright file="CreateAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.Services.Crm;
using AllyisApps.ViewModels.TimeTracker.Project;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// The controller for managing all Project-related actions.
	/// </summary>
	public partial class ProjectController : BaseController
	{
		/// <summary>
		/// GET: Project/Create.
		/// Gets the page for creating new projects.
		/// </summary>
		/// <param name="subscriptionId">Subscription id.</param>
		/// <returns>The ActionResult for the Create view.</returns>
		[HttpGet]
		public async Task<ActionResult> Create(int subscriptionId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, subscriptionId);
			var orgId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId;
			var idAndUsers = await AppService.GetNextProjectId(orgId, subscriptionId);
			var customers = (await AppService.GetCustomerList(orgId)).Where(x => x.IsActive.Value);
			var subUsers = AppService.GetSubscriptionUsers(subscriptionId).AsEnumerable();
			var customersActive = customers.Select(x => new SelectListItem()
			{
				Text = x.CustomerName,
				Value = x.CustomerId.ToString()
			}).ToList();

			List<SelectListItem> statusOptions = new List<SelectListItem>()
			{
				new SelectListItem() { Text = "Active", Value = true.ToString() },
				new SelectListItem() { Text = "Disabled", Value = false.ToString() }
			};

			string subscriptionNameToDisplay = await AppService.GetSubscriptionName(subscriptionId);
			return View(
				new EditProjectViewModel
				{
					IsCreating = true,
					ProjectUsers = new List<BasicUserInfoViewModel>(),
					ProjectCode = idAndUsers, // Service.GetRecommendedProjectId()
					SubscriptionId = subscriptionId,
					Customers = customersActive,
					IsActive = true,
					isActiveOptions = statusOptions,
					SubscriptionName = subscriptionNameToDisplay,
					SubscriptionUsers = subUsers.Select(x => new BasicUserInfoViewModel(x.FirstName, x.LastName, x.UserId)),
					OrganizationId = AppService.UserContext.SubscriptionsAndRoles[subscriptionId].OrganizationId
				});
		}

		/// <summary>
		/// POST: Project/Create
		/// Method for creating a new project entry in the database.
		/// </summary>
		/// <param name="model">The model of user input.</param>
		/// <returns>If successful, notifies and redirects to Project/Index. Else, returns to the create project form.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(EditProjectViewModel model)
		{
			var listGet = await AppService.GetNextProjectIdAndSubUsers(model.ParentCustomerId, model.SubscriptionId);
			model.SubscriptionUsers = listGet.Item2.Select(user => new BasicUserInfoViewModel(user.FirstName, user.LastName, user.UserId)).ToList();

			if (!ModelState.IsValid) return View(model);

			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditProject, model.SubscriptionId);

			try
			{
				var result = await CreateProjectAndUpdateItsUserList(model);
				if (result == -1)
				{
					// duplicate projectCode
					Notifications.Add(new BootstrapAlert(Resources.Strings.ProjectCodeNotUnique, Variety.Danger));
				}
				else
				{
					model.ProjectId = result;
					Notifications.Add(new BootstrapAlert(Resources.Strings.SuccessProjectCreated, Variety.Success));
				}
			}
			catch (Exception ex)
			{
				// Create failure
				Notifications.Add(new BootstrapAlert($"{Resources.Strings.FailureProjectCreated} {ex.Message}", Variety.Danger));
				return View(model);
			}

			return RedirectToAction(ActionConstants.Index, ControllerConstants.Project, new { subscriptionId = model.SubscriptionId });

			// Invalid Model
		}

		/// <summary>
		/// Creates a new project using a <see cref="EditProjectViewModel"/> and updates that project's properties and user list..
		/// </summary>
		/// <param name="model"><see cref="EditProjectViewModel"/> representing new project.</param>
		/// <returns>The Project Id if succeed, -1 if the ProjectCode is taken by another project under the same customer.</returns>
		public async Task<int> CreateProjectAndUpdateItsUserList(EditProjectViewModel model)
		{
			return await AppService.CreateProjectAndUpdateItsUserList(ProjectViewModelToProject(model), model.SelectedProjectUserIds.Select(int.Parse));
		}

		/// <summary>
		/// Creates a new project using a <see cref="EditProjectViewModel"/>.
		/// </summary>
		/// <param name="model"><see cref="EditProjectViewModel"/> representing new project.</param>
		/// <returns>The Project Id.</returns>
		public async Task<int> CreateProject(EditProjectViewModel model) => await AppService.CreateProject(ProjectViewModelToProject(model));

		private static Services.Project.Project ProjectViewModelToProject(EditProjectViewModel model)
		{
			return new Services.Project.Project
			{
				owningCustomer = new Customer
				{
					CustomerId = model.CustomerId,
				},
				ProjectName = model.ProjectName,
				ProjectCode = model.ProjectCode,
				StartingDate = model.StartDate,
				EndingDate = model.EndDate
			};
		}
	}
}