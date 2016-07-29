//------------------------------------------------------------------------------
// <copyright file="ReportAction.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using AllyisApps.Areas.TimeTracker.Models;
using AllyisApps.Core;
using AllyisApps.Services.BusinessObjects;

namespace AllyisApps.Areas.TimeTracker.Controllers
{
	/// <summary>
	/// Class which manages Time Entry objects.
	/// </summary>
	public partial class TimeEntryController : BaseProductController
	{
		/// <summary>
		/// GET /TimeTracker/TimeEntry/Report.
		/// </summary>
		/// <param name="organizationId">The Organization's Id.</param>
		/// <returns>The reports page.</returns>
		public ViewResult Report(int organizationId = 0)
		{
			int orgId = UserContext.ChosenOrganizationId;
			ReportViewModel reportVM = null;

			const string tempDataKey = "RVM";
			if (this.TempData[tempDataKey] != null)
			{
				reportVM = (ReportViewModel)TempData[tempDataKey];
				orgId = reportVM.OrganizationId;
			}
			else
			{
				reportVM = this.ConstructReportViewModel(UserContext.UserId, orgId, AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditOthers));
			}

			if (!AuthorizationService.Can(Services.Account.Actions.CoreAction.TimeTrackerEditSelf))
			{
				throw new UnauthorizedAccessException(AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.UnauthorizedReports);
			}

			reportVM.StartOfWeek = TimeTrackerService.GetStartOfWeek(UserContext.ChosenOrganizationId);
			reportVM.UserView = this.GetUserSelectList(orgId, reportVM.Selection.Users);
			reportVM.CustomerView = this.GetCustomerSelectList(orgId, reportVM.Selection.CustomerId);
			reportVM.ProjectView = this.GetProjectSelectList(orgId, reportVM.Selection.CustomerId, reportVM.Selection.ProjectId);
			reportVM.PreviewPageNum = reportVM.Selection.Page;

			return this.View(reportVM);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReportViewModel" /> class.
		/// </summary>
		/// <param name="userId">The users's ID.</param>
		/// <param name="organizationId">The organization's ID.</param>
		/// <param name="canManage">Is user a manager.</param>
		/// <param name="showExport">Variable to show or hide export button.</param>
		/// <param name="previousSelections">An object holding previous report selection data.</param>
		/// <returns>The ReportViewModel.</returns>
		public ReportViewModel ConstructReportViewModel(int userId, int organizationId, bool canManage, bool showExport = true, ReportSelectionModel previousSelections = null)
		{
			return new ReportViewModel
			{
				UserId = userId,
				CanManage = canManage,
				OrganizationId = organizationId,
				ShowExport = showExport,
				Projects = OrgService.GetProjectsByOrganization(organizationId, false),
				Customers = CrmService.GetCustomerList(organizationId),
				PreviewPageSize = 20,
				PreviewTotal = string.Format("{0} {1}", 0, AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.HoursTotal),
				PreviewEntries = null,
				PreviewMessage = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.NoDataPreview,
				PreviewPageTotal = 1,
				PreviewPageNum = 1,
				Selection = previousSelections ?? new ReportSelectionModel
				{
					CustomerId = 0,
					EndDate = DateTime.Today,
					Page = 1,
					ProjectId = 0,
					StartDate = DateTime.Today,
					Users = new List<int>()
				}
			};
		}

		/// <summary>
		/// Fills the list of users and sets selection.
		/// </summary>
		/// <param name="organizationId">The Organization's Id.</param>
		/// <param name="usersSelected">The list of selected users.</param>
		/// <returns>The user list.</returns>
		private IEnumerable<SelectListItem> GetUserSelectList(int organizationId, List<int> usersSelected)
		{
			IList<UserInfo> users = (IList<UserInfo>)CrmService.GetUsersWithSubscriptionToProductInOrganization(organizationId, Services.Crm.CrmService.GetProductIdByName("TimeTracker")).ToList<UserInfo>();
			users.Insert(0, new UserInfo { FirstName = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.AllUsersFirst, LastName = AllyisApps.Resources.TimeTracker.Controllers.TimeEntry.Strings.AllUsersLast, UserId = -1 });

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
		/// <param name="organizationId">The Organization's Id.</param>
		/// <param name="customerSelected">The selected customer.</param>
		/// <returns>The customer list.</returns>
		private IEnumerable<SelectListItem> GetCustomerSelectList(int organizationId, int customerSelected)
		{
			IList<CustomerInfo> customerData = CrmService.GetCustomerList(organizationId).ToList<CustomerInfo>();
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
		/// <param name="organizationId">The Organization's Id.</param>
		/// <param name="customerSelected">The selected customer.</param>
		/// <param name="projectSelected">The selected project.</param>
		/// <returns>The project list.</returns>
		private IEnumerable<SelectListItem> GetProjectSelectList(int organizationId, int customerSelected, int projectSelected)
		{
			var pSelectList = new List<SelectListItem>();
			
			// disable project selection if no customer selected
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

				IList<ProjectInfo> projectData = ProjectService.GetProjectsByCustomer(customerSelected).ToList<ProjectInfo>();
				foreach (var project in projectData)
				{
					pSelectList.Add(new SelectListItem
					{
						Value = project.ProjectId.ToString(),
						Text = project.Name,
						Selected = project.ProjectId == projectSelected
					});
				}
			}

			return pSelectList;
		}
	}
}