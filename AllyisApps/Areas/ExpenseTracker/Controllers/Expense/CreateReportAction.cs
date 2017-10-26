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
	/// Stores a new report in database and redirects to View.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		/// <summary>
		/// View/export expense report.
		/// </summary>
		/// <param name="model">The model object.</param>
		/// <returns>An action result.</returns>
		[HttpPost]
		public async Task<ActionResult> CreateReport(ExpenseCreateModel model)
		{
			AppService.CheckExpenseTrackerAction(AppService.ExpenseTrackerAction.Unmanaged, model.SubscriptionId);

			if (!ModelState.IsValid)
			{
				return RedirectToAction("Create", new { subscriptionId = model.SubscriptionId, reportId = model.Report.ExpenseReportId });
			}

			if (AppService.UserContext.UserId != model.CurrentUser)
			{
				string message = string.Format("action {0} denied", AppService.ExpenseTrackerAction.CreateReport);
				throw new AccessViolationException(message);
			}

			if (model.Items == null)
			{
				model.Items = new List<ExpenseItemCreateViewModel>();
			}

			var subscription = await AppService.GetSubscription(model.SubscriptionId);
			var organizationId = subscription.OrganizationId;
			ExpenseStatusEnum reportStatus;
			DateTime? submittedUtc = null;

			if (model.SubmitType == "Submit")
			{
				reportStatus = ExpenseStatusEnum.Pending;
				submittedUtc = DateTime.UtcNow;
			}
			else
			{
				reportStatus = ExpenseStatusEnum.Draft;
			}

			var report = new ExpenseReport
			{
				ReportTitle = model.Report.ReportTitle,
				BusinessJustification = model.Report.BusinessJustification,
				CreatedUtc = DateTime.UtcNow,
				ModifiedUtc = DateTime.UtcNow,
				SubmittedUtc = submittedUtc,
				SubmittedById = model.CurrentUser,
				OrganizationId = organizationId,
				ReportStatus = (int)reportStatus
			};

			report.ExpenseReportId = AppService.CreateExpenseReport(report);
			await UploadItems(model, report);
			UploadAttachments(model, report);

			return RedirectToAction("Index");
		}
	}
}