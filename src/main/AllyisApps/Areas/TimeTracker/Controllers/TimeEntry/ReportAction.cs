﻿//------------------------------------------------------------------------------
// <copyright file="ReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.Core;
using AllyisApps.Core.Alert;
using AllyisApps.Services;
using AllyisApps.ViewModels.TimeTracker.TimeEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
		/// <returns>The reports page.</returns>
		public ActionResult Report()
		{
			if (Service.Can(Actions.CoreAction.TimeTrackerEditOthers))
			{
				if (!Service.Can(Actions.CoreAction.TimeTrackerEditSelf))
				{
					throw new UnauthorizedAccessException(AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.UnauthorizedReports);
				}

				int orgId = UserContext.ChosenOrganizationId;
				ReportViewModel reportVM = null;

				var infos = TimeTrackerService.GetReportInfo(orgId);

				const string TempDataKey = "RVM";
				if (this.TempData[TempDataKey] != null)
				{
					reportVM = (ReportViewModel)TempData[TempDataKey];
					orgId = reportVM.OrganizationId;
				}
				else
				{
					reportVM = this.ConstructReportViewModel(UserContext.UserId, orgId, Service.Can(Actions.CoreAction.TimeTrackerEditOthers), infos.Item1, infos.Item2);
				}
				
				//reportVM.StartOfWeek = (int)TimeTrackerService.GetStartOfWeek();
				reportVM.UserView = this.GetUserSelectList(infos.Item3/*orgId*/, reportVM.Selection.Users);
				reportVM.CustomerView = this.GetCustomerSelectList(infos.Item1/*orgId*/, reportVM.Selection.CustomerId);
				reportVM.ProjectView = this.GetProjectSelectList(infos.Item2/*orgId*/, reportVM.Selection.CustomerId, reportVM.Selection.ProjectId);
				//reportVM.PreviewPageNum = reportVM.Selection.Page;

				return this.View(reportVM);
			}

			// Permissions failure
			Notifications.Add(new BootstrapAlert(Resources.Errors.ActionUnauthorizedMessage, Variety.Warning));

			return this.RouteHome();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReportViewModel" /> class.
		/// </summary>
		/// <param name="userId">The users's ID.</param>
		/// <param name="organizationId">The organization's ID.</param>
		/// <param name="canManage">Is user a manager.</param>
		/// <param name="customers">List of CustomerInfos for organization.</param>
		/// <param name="projects">List of CompleteProjectInfos for organization.</param>
		/// <param name="showExport">Variable to show or hide export button.</param>
		/// <param name="previousSelections">An object holding previous report selection data.</param>
		/// <returns>The ReportViewModel.</returns>
		public ReportViewModel ConstructReportViewModel(int userId, int organizationId, bool canManage, List<CustomerInfo> customers, List<CompleteProjectInfo> projects, bool showExport = true, ReportSelectionModel previousSelections = null)
		{
			return new ReportViewModel
			{
				UserId = userId,
				CanManage = canManage,
				OrganizationId = organizationId,
				ShowExport = showExport,
				Projects = projects, //Service.GetProjectsByOrganization(organizationId, false),
				Customers = customers, //Service.GetCustomerList(organizationId),
				PreviewPageSize = 20,
				PreviewTotal = string.Format("{0} {1}", 0, AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.HoursTotal),
				PreviewEntries = null,
				PreviewMessage = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.NoDataPreview,
				PreviewPageTotal = 1,
				PreviewPageNum = 1,
				Selection = previousSelections ?? new ReportSelectionModel
				{
					CustomerId = 0,
					EndDate = TimeTrackerService.GetDayFromDateTime(DateTime.Today),
					Page = 1,
					ProjectId = 0,
					StartDate = TimeTrackerService.GetDayFromDateTime(DateTime.Today),
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
		private IEnumerable<SelectListItem> GetUserSelectList(List<SubscriptionUserInfo> subUsers/*int organizationId*/, List<int> usersSelected)
		{
			IList<SubscriptionUserInfo> users = subUsers;//(IList<UserInfo>)Service.GetUsersWithSubscriptionToProductInOrganization(organizationId, Service.GetProductIdByName(ProductNameKeyConstants.TimeTracker)).ToList<UserInfo>();
			users.Insert(0, new SubscriptionUserInfo { FirstName = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.AllUsersFirst, LastName = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.AllUsersLast, UserId = -1 });

			// select current user by default
			if (usersSelected.Count < 1)
			{
				usersSelected.Add(Convert.ToInt32(UserContext.UserId));
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
		private IEnumerable<SelectListItem> GetCustomerSelectList(List<CustomerInfo> customers/*int organizationId*/, int customerSelected)
		{
			IList<CustomerInfo> customerData = customers;//Service.GetCustomerList(organizationId).ToList<CustomerInfo>();
			customerData.Insert(0, new CustomerInfo { Name = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.NoFilter, CustomerId = 0 });

			var cSelectList = new List<SelectListItem>();
			foreach (var customer in customerData)
			{
				cSelectList.Add(new SelectListItem
				{
					Value = customer.CustomerId.ToString(),
					Text = customer.Name,
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
		private IEnumerable<SelectListItem> GetProjectSelectList(List<CompleteProjectInfo> projects/*int organizationId*/, int customerSelected, int projectSelected)
		{
			var pSelectList = new List<SelectListItem>();

			//disable project selection if no customer selected
			if (customerSelected == 0)
			{
				pSelectList.Add(new SelectListItem
				{
					Value = 0.ToString(),
					Text = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.NoFilter,
					Selected = true,
					Disabled = true
				});
			}
			else
			{
				pSelectList.Add(new SelectListItem
				{
					Value = 0.ToString(),
					Text = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.NoFilter,
					Selected = projectSelected == 0,
					Disabled = false
				});

				//IList<ProjectInfo> projectData = Service.GetProjectsByCustomer(customerSelected).ToList<ProjectInfo>();
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
