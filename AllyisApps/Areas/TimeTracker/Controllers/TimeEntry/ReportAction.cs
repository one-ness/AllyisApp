//------------------------------------------------------------------------------
// <copyright file="ReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Crm;
using AllyisApps.Services.TimeTracker;
using AllyisApps.ViewModels.TimeTracker.Project;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseController
	{
		/// <summary>
		/// GET /TimeTracker/TimeEntry/Report.
		/// </summary>
		/// <param name="subscriptionId">The Subscription Id.</param>
		/// <returns>The reports page.</returns>
		public async Task<ActionResult> Report(int subscriptionId)
		{
			AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);

			ReportViewModel reportVM;

			var infos = await AppService.GetReportInfo(subscriptionId);

			const string tempDataKey = "RVM";
			AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out UserContext.SubscriptionAndRole subInfo);

			if (TempData[tempDataKey] != null)
			{
				reportVM = (ReportViewModel)TempData[tempDataKey];
			}
			else
			{
				reportVM = await ConstructReportViewModel(AppService.UserContext.UserId, subInfo.OrganizationId, true, infos.Customers, infos.CompleteProject);
				reportVM.SubscriptionName = subInfo.SubscriptionName;
			}

			reportVM.UserView = GetUserSelectList(infos.SubscriptionUserInfo, reportVM.Selection.Users);
			reportVM.CustomerView = GetCustomerSelectList(infos.Customers, reportVM.Selection.CustomerId);
			reportVM.ProjectView = GetProjectSelectList(infos.CompleteProject, reportVM.Selection.CustomerId, reportVM.Selection.ProjectId);
			reportVM.SubscriptionId = subscriptionId;

			return View(reportVM);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReportViewModel" /> class.
		/// </summary>
		/// <param name="userId">The users's Id.</param>
		/// <param name="organizationId">The organization's Id.</param>
		/// <param name="canManage">Is user a manager.</param>
		/// <param name="customers">List of Customers for organization.</param>
		/// <param name="projects">List of CompleteProjectInfos for organization.</param>
		/// <param name="showExport">Variable to show or hide export button.</param>
		/// <param name="previousSelections">An object holding previous report selection data.</param>
		/// <returns>The ReportViewModel.</returns>
		public async Task<ReportViewModel> ConstructReportViewModel(int userId, int organizationId, bool canManage, List<Customer> customers, List<CompleteProject> projects, bool showExport = true, ReportSelectionModel previousSelections = null)
		{
			projects.Insert(
				0,
				new CompleteProject
				{
					ProjectId = 0,
					ProjectName = Resources.Strings.NoFilter,
					OwningCustomer = new Customer
					{
						CustomerId = 0
					}
				});

			PayPeriodRanges payPeriods = await AppService.GetPayPeriodRanges(organizationId);

			return new ReportViewModel
			{
				UserId = userId,
				CanManage = canManage,
				OrganizationId = organizationId,
				ShowExport = showExport,
				Projects = projects.AsParallel().Select(proj => new CompleteProjectViewModel(proj)).ToList(),
				PreviewPageSize = 20,
				PreviewTotal = $"0 {Resources.Strings.HoursTotal}",
				PreviewEntries = null,
				PreviewMessage = Resources.Strings.NoDataPreview,
				PreviewPageTotal = 1,
				PreviewPageNum = 1,
				PayPeriodRanges = payPeriods,
				Selection = previousSelections ?? new ReportSelectionModel
				{
					Users = new List<int>(),
					CustomerId = 0,
					ProjectId = 0,
					StartDate = payPeriods.Previous.StartDate,
					EndDate = payPeriods.Previous.EndDate,
					Page = 1
				}
			};
		}

		/// <summary>
		/// Fills the list of users and sets selection.
		/// </summary>
		/// <param name="subUsers">List of subscription users.</param>
		/// <param name="usersSelected">List of selected user ids.</param>
		/// <returns>The user list.</returns>
		private List<SelectListItem> GetUserSelectList(IList<SubscriptionUser> subUsers, List<int> usersSelected)
		{
			// ReSharper disable once SuggestVarOrType_Elsewhere
			var users = subUsers;
			users.Insert(0, new SubscriptionUser { FirstName = Resources.Strings.AllUsersFirst, LastName = Resources.Strings.AllUsersLast, UserId = -1 });

			// select current user by default
			if (usersSelected.Count < 1)
			{
				usersSelected.Add(Convert.ToInt32(AppService.UserContext.UserId));
			}

			return users.Select(user => new SelectListItem
			{
				Value = user.UserId.ToString(),
				Text = $"{user.FirstName} {user.LastName}",
				Selected = usersSelected.Contains(user.UserId)
			}).ToList();
		}

		/// <summary>
		/// Fills the list of customers and sets selection.
		/// </summary>
		/// <param name="customers">The list of customers.</param>
		/// <param name="customerSelected">The selected customer.</param>
		/// <returns>The customer list.</returns>
		private static List<SelectListItem> GetCustomerSelectList(IList<Customer> customers, int customerSelected)
		{
			var customerData = customers;
			customerData.Insert(0, new Customer { CustomerName = Resources.Strings.NoFilter, CustomerId = 0 });

			return customerData.Select(customer => new SelectListItem
			{
				Value = customer.CustomerId.ToString(),
				Text = customer.CustomerName,
				Selected = customer.CustomerId == customerSelected
			})
				.ToList();
		}

		/// <summary>
		/// Fills the list of projects and sets selection.
		/// </summary>
		/// <param name="projects">The list of all projects.</param>
		/// <param name="customerSelected">The selected customer.</param>
		/// <param name="projectSelected">The selected project.</param>
		/// <returns>The project list.</returns>
		private static List<SelectListItem> GetProjectSelectList(List<CompleteProject> projects, int customerSelected, int projectSelected)
		{
			var pSelectList = new List<SelectListItem>();

			// disable project selection if no customer selected
			if (customerSelected == 0)
			{
				pSelectList.Add(new SelectListItem
				{
					Value = 0.ToString(),
					Text = Resources.Strings.NoFilter,
					Selected = true,
					Disabled = true
				});
			}
			else
			{
				pSelectList.Add(new SelectListItem
				{
					Value = 0.ToString(),
					Text = Resources.Strings.NoFilter,
					Selected = projectSelected == 0,
					Disabled = false
				});

				var projectData = projects.Where(cpi => cpi.OwningCustomer.CustomerId == customerSelected).ToList();
				pSelectList.AddRange(projectData.Select(project => new SelectListItem
				{
					Value = project.ProjectId.ToString(),
					Text = project.ProjectName,
					Selected = project.ProjectId == projectSelected
				}));
			}

			return pSelectList;
		}
	}
}