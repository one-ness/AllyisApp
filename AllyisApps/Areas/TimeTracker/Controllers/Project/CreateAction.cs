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
			var customersActive = customers.Select(x => new SelectListItem
			{
				Text = x.CustomerName,
				Value = x.CustomerId.ToString()
			}).ToList();

			string subscriptionNameToDisplay = await AppService.GetSubscriptionName(subscriptionId);
			return View(
				new EditProjectViewModel
				{
					IsCreating = true,
					ProjectUsers = new List<BasicUserInfoViewModel>(),
					ProjectCode = idAndUsers, // Service.GetRecommendedProjectId()
					SubscriptionId = subscriptionId,
					Customers = customersActive,
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
			//for repopulating form choices upon edit error
			var totalUsers = model.ProjectUsers.Concat(model.SubscriptionUsers).ToList();
			model.ProjectUsers = totalUsers.Where(subUsers => model.SelectedProjectUserIds.Contains(subUsers.UserId)); //right multi-select box
			model.SubscriptionUsers = totalUsers.Except(model.ProjectUsers); //left multi-select box

			// Invalid Model
			if (!ModelState.IsValid) return View(model);

			//model.IsActive = (model.StartDate == null || model.StartDate <= DateTime.Now)
			//				&& (model.EndDate == null || model.EndDate >= DateTime.Now);

			try
			{
				int result = await AppService.CreateProjectAndUpdateItsUserList(ProjectViewModelToProject(model), model.SelectedProjectUserIds.Select(int.Parse), model.SubscriptionId);
				Notifications.Add(result == -1
					? new BootstrapAlert(Resources.Strings.ProjectCodeNotUnique, Variety.Danger)
					: new BootstrapAlert(Resources.Strings.SuccessProjectCreated, Variety.Success));
			}
			catch (Exception ex)
			{
				// Create failure
				Notifications.Add(new BootstrapAlert($"{Resources.Strings.FailureProjectCreated} {ex.Message}", Variety.Danger));
				return View(model);
			}

			return RedirectToAction(ActionConstants.Index, ControllerConstants.Project, new { subscriptionId = model.SubscriptionId });
		}

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