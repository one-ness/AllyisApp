﻿using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.ViewModels.ExpenseTracker.Expense;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// The ExpenseController class with ReportView actions.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// The ReportView Action.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="reportId">The report id.</param>
		/// <returns>The view with selected report details.</returns>
		public ActionResult ReportView(int subscriptionId, int reportId)
		{
			SetNavData(subscriptionId);

			var model = InitializeReportViewModel(subscriptionId, reportId);

			return View(model);
		}

		/// <summary>
		/// Initializes the ReportViewModel.
		/// </summary>
		/// <param name="subscriptionId">The subscription id.</param>
		/// <param name="id">The report id.</param>
		/// <returns>The view model.</returns>
		public ReportViewModel InitializeReportViewModel(int subscriptionId, int id)
		{
			UserContext.SubscriptionAndRole subInfo = this.AppService.UserContext.SubscriptionsAndRoles[subscriptionId];
			ViewData["IsManager"] = subInfo.ProductRoleId == 2;
			ViewData["SubscriptionId"] = subscriptionId;

			var report = AppService.GetExpenseReport(id);
			var reportItems = AppService.GetExpenseItemsByReportId(id);
			var user = AppService.GetUserInfo(report.SubmittedById);
			var history = AppService.GetExpenseHistoryByReportId(id);
			List<ExpenseHistoryViewModel> reportHistory = new List<ExpenseHistoryViewModel>();

			List<string> fileNames = AzureFiles.GetReportAttachments(id);

			foreach (var item in history)
			{
				var reviewer = AppService.GetUserInfo(item.UserId);
				reportHistory.Add(new ExpenseHistoryViewModel()
				{
					Reviewer = string.Format("{0} {1}", reviewer.FirstName, reviewer.LastName),
					Status = (ExpenseStatusEnum)item.Status,
					Submitted = item.CreatedUtc,
					Text = item.Text
				});
			}

			return new ReportViewModel()
			{
				ReprortTitle = report.ReportTitle,
				SubmittedBy = string.Format("{0} {1}", user.FirstName, user.LastName),
				CreatedUtc = report.CreatedUtc,
				ModifiedUtc = report.ModifiedUtc,
				SubmittedUtc = report.SubmittedUtc,
				Justification = report.BusinessJustification,
				Status = (ExpenseStatusEnum)report.ReportStatus,
				Expenses = reportItems,
				History = reportHistory,
				UserId = user.UserId,
				ReportId = report.ExpenseReportId,
				SubscriptionId = subscriptionId,
				Attachments = fileNames
			};
		}
	}
}