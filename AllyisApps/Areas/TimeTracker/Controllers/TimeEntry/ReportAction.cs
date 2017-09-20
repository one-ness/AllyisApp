//------------------------------------------------------------------------------
// <copyright file="ReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.Project;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
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
		public ActionResult Report(int subscriptionId)
		{
			this.AppService.CheckTimeTrackerAction(AppService.TimeTrackerAction.EditOthers, subscriptionId);

			ReportViewModel reportVM = null;

			var infos = AppService.GetReportInfo(subscriptionId);

			const string TempDataKey = "RVM";
			UserContext.SubscriptionAndRole subInfo = null;
			this.AppService.UserContext.SubscriptionsAndRoles.TryGetValue(subscriptionId, out subInfo);
			string subName = AppService.GetSubscription(subscriptionId).Name;
			if (this.TempData[TempDataKey] != null)
			{
				reportVM = (ReportViewModel)TempData[TempDataKey];
			}
			else
			{
				reportVM = this.ConstructReportViewModel(this.AppService.UserContext.UserId, subInfo.OrganizationId, true, infos.Item1, infos.Item2);
				reportVM.SubscriptionName = subName;
			}

			reportVM.UserView = this.GetUserSelectList(infos.Item3, reportVM.Selection.Users);
			reportVM.CustomerView = this.GetCustomerSelectList(infos.Item1, reportVM.Selection.CustomerId);
			reportVM.ProjectView = this.GetProjectSelectList(infos.Item2, reportVM.Selection.CustomerId, reportVM.Selection.ProjectId);
			reportVM.SubscriptionId = subscriptionId;

			var infoOrg = AppService.GetTimeEntryIndexInfo(subInfo.OrganizationId, null, null);
			ViewBag.WeekStart = AppService.GetDaysFromDateTime(AppService.SetStartingDate(null, infoOrg.Item1.StartOfWeek));
			ViewBag.WeekEnd = AppService.GetDaysFromDateTime(SetEndingDate(null, infoOrg.Item1.StartOfWeek));

			return this.View(reportVM);
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
		public ReportViewModel ConstructReportViewModel(int userId, int organizationId, bool canManage, List<Customer> customers, List<CompleteProjectInfo> projects, bool showExport = true, ReportSelectionModel previousSelections = null)
		{
			projects.Insert(
				0,
				new CompleteProjectInfo
				{
					ProjectId = 0,
					ProjectName = AllyisApps.Resources.Strings.NoFilter,
					CustomerId = 0
				});

			return new ReportViewModel
			{
				UserId = userId,
				CanManage = canManage,
				OrganizationId = organizationId,
				ShowExport = showExport,
				Projects = projects.AsParallel().Select(proj => new CompleteProjectViewModel(proj)).AsEnumerable(),
				PreviewPageSize = 20,
				PreviewTotal = string.Format("{0} {1}", 0, Resources.Strings.HoursTotal),
				PreviewEntries = null,
				PreviewMessage = AllyisApps.Resources.Strings.NoDataPreview,
				PreviewPageTotal = 1,
				PreviewPageNum = 1,
				Selection = previousSelections ?? new ReportSelectionModel
				{
					CustomerId = 0,
					EndDate = AppService.GetDaysFromDateTime(DateTime.Today),
					Page = 1,
					ProjectId = 0,
					StartDate = AppService.GetDaysFromDateTime(DateTime.Today),
					Users = new List<int>()
				}
			};
		}

		/// <summary>
		/// Fills the list of users and sets selection.
		/// </summary>
		/// <param name="subUsers">List of subscription users.</param>
		/// <param name="usersSelected">List of selected user ids.</param>
		/// <returns>The user list.</returns>
		private IEnumerable<SelectListItem> GetUserSelectList(List<SubscriptionUserInfo> subUsers, List<int> usersSelected)
		{
			IList<SubscriptionUserInfo> users = subUsers;
			users.Insert(0, new SubscriptionUserInfo { FirstName = Resources.Strings.AllUsersFirst, LastName = Resources.Strings.AllUsersLast, UserId = -1 });

			// select current user by default
			if (usersSelected.Count < 1)
			{
				usersSelected.Add(Convert.ToInt32(this.AppService.UserContext.UserId));
			}

			var selectList = new List<SelectListItem>();
			foreach (var user in users)
			{
				selectList.Add(new SelectListItem
				{
					Value = user.UserId.ToString(),
					Text = string.Format("{0} {1}", user.FirstName, user.LastName),
					Selected = usersSelected.Contains(user.UserId)
				});
			}

			return selectList;
		}

		/// <summary>
		/// Fills the list of customers and sets selection.
		/// </summary>
		/// <param name="customers">The list of customers.</param>
		/// <param name="customerSelected">The selected customer.</param>
		/// <returns>The customer list.</returns>
		private IEnumerable<SelectListItem> GetCustomerSelectList(List<Customer> customers, int customerSelected)
		{
			IList<Customer> customerData = customers;
			customerData.Insert(0, new Customer { CustomerName = Resources.Strings.NoFilter, CustomerId = 0 });

			var cSelectList = new List<SelectListItem>();
			foreach (var customer in customerData)
			{
				cSelectList.Add(new SelectListItem
				{
					Value = customer.CustomerId.ToString(),
					Text = customer.CustomerName,
					Selected = customer.CustomerId == customerSelected
				});
			}

			return cSelectList;
		}

		/// <summary>
		/// Fills the list of projects and sets selection.
		/// </summary>
		/// <param name="projects">The list of all projects.</param>
		/// <param name="customerSelected">The selected customer.</param>
		/// <param name="projectSelected">The selected project.</param>
		/// <returns>The project list.</returns>
		private IEnumerable<SelectListItem> GetProjectSelectList(List<CompleteProjectInfo> projects, int customerSelected, int projectSelected)
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

				List<CompleteProjectInfo> projectData = projects.Where(cpi => cpi.CustomerId == customerSelected).ToList();
				foreach (var project in projectData)
				{
					pSelectList.Add(new SelectListItem
					{
						Value = project.ProjectId.ToString(),
						Text = project.ProjectName,
						Selected = project.ProjectId == projectSelected
					});
				}
			}

			return pSelectList;
		}
	}
}