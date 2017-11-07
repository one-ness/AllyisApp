using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AllyisApps.Controllers;
using AllyisApps.Services;
using AllyisApps.Services.Expense;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// The expense controller.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// The Admin report action.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <returns>A admin report view model.</returns>
		public async Task<ActionResult> AdminReport(int subscriptionId)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.AdminReport, subscriptionId);

			await SetNavData(subscriptionId);

			AdminReportModel adminReportVM = null;

			string tempDataKey = "ARVM";

			if (TempData[tempDataKey] != null)
			{
				adminReportVM = (AdminReportModel)TempData[tempDataKey];
			}
			else
			{
				adminReportVM = await CreateAdminReportModel(subscriptionId);
			}

			return View(adminReportVM);
		}

		/// <summary>
		/// Creates an admin report view model.
		/// </summary>
		/// <param name="subId">The subscription id.</param>
		/// <returns>An admin report model.</returns>
		public async Task<AdminReportModel> CreateAdminReportModel(int subId)
		{
			
			var reportInfoTask = AppService.GetReportInfo(subId);

			await Task.WhenAll(new Task[] {reportInfoTask });

			var subInfo = AppService.UserContext.SubscriptionsAndRoles[subId];
			var reportInfo = reportInfoTask.Result;

			var reports = await AppService.GetExpenseReportByOrgId(subInfo.OrganizationId);
			List<ExpenseReportViewModel> reportViewModels = new List<ExpenseReportViewModel>();
			foreach (ExpenseReport report in reports)
			{
				reportViewModels.Add(InitializeExpenseReportViewModel(report));
			}

			var users = reportInfo.SubscriptionUserInfo;
			List<SelectListItem> enumList = new List<SelectListItem>();
			List<SelectListItem> userList = new List<SelectListItem>();

			foreach (var item in Enum.GetValues(typeof(ExpenseStatusEnum)))
			{
				int value = 0;
				switch (item.ToString())
				{
					// We dont want reports on rough drafts for now.
					case "Pending":
						value = 1;
						break;

					case "Approved":
						value = 2;
						break;

					case "Rejected":
						value = 3;
						break;

					case "Paid":
						value = 4;
						break;

					default:
						break;
				}

				if (value != 0)
				{
					enumList.Add(new SelectListItem
					{
						Disabled = false,
						Text = item.ToString(),
						Value = value.ToString(),
						Selected = string.Equals("1", value.ToString()) // Set pending as the default status.
					});
				}
			}

			foreach (var user in users)
			{
				userList.Add(new SelectListItem
				{
					Disabled = false,
					Text = string.Format("{0} {1}", user.FirstName, user.LastName),
					Value = user.UserId.ToString(),
					Selected = false
				});
			}

			AdminReportModel model = new AdminReportModel
			{
				CanManage = true,
				OrganizationId = subInfo.OrganizationId,
				Reports = reportViewModels,
				PreviewReports = null,
				ShowExport = true,
				Statuses = enumList,
				SubscriptionId = subInfo.SubscriptionId,
				SubscriptionName = subInfo.SubscriptionName,
				UserId = AppService.UserContext.UserId,
				Users = userList,
				Selection = new AdminReportSelectionModel
				{
					EndDate = DateTime.Today.AddDays(7),
					StartDate = DateTime.Today,
					SelectedUsers = new List<int>(),
					Status = new List<int>()
				}
			};

			return model;
		}

		private ExpenseReportViewModel InitializeExpenseReportViewModel(ExpenseReport report)
		{
			return report == null ? null : new ExpenseReportViewModel
			{
				BusinessJustification = report.BusinessJustification,
				CreatedUtc = report.CreatedUtc,
				ExpenseReportId = report.ExpenseReportId,
				ModifiedUtc = report.ModifiedUtc,
				OrganizationId = report.OrganizationId,
				ReportStatus = report.ReportStatus,
				ReportTitle = report.ReportTitle,
				SubmittedById = report.SubmittedById,
				SubmittedUtc = report.SubmittedUtc
			};
		}
	}
}