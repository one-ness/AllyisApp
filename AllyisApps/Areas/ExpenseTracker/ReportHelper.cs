using System.Collections.Generic;
using System.Linq;
using AllyisApps.Controllers;
using AllyisApps.Lib;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Expense;
using AllyisApps.ViewModels.ExpenseTracker.Expense;

namespace AllyisApps.Areas.ExpenseTracker.Controllers
{
	/// <summary>
	/// Stores a new report in database and redirects to View.
	/// </summary>
	public partial class ExpenseController : BaseController
	{
		private void SetNavData(int subscriptionId)
		{
			UserContext.SubscriptionAndRole subInfo = AppService.UserContext.SubscriptionsAndRoles[subscriptionId];
			ViewData["SubscriptionName"] = AppService.GetSubscriptionName(subscriptionId);
			ViewData["SubscriptionId"] = subscriptionId;
			ViewData["ProductRole"] = subInfo.ProductRoleId;
			ViewData["MaxAmount"] = AppService.GetOrganizationUserMaxAmount(AppService.UserContext.UserId, subInfo.OrganizationId);
		}

		private void UploadItems(ExpenseCreateModel model, ExpenseReport report)
		{
			IList<ExpenseItem> oldItems = AppService.GetExpenseItemsByReportId(report.ExpenseReportId);
			List<int> itemIds = new List<int>();
			foreach (ExpenseItem oldItem in oldItems)
			{
				itemIds.Add(oldItem.ExpenseItemId);
			}

			foreach (var itemViewModel in model.Items)
			{
				ExpenseItem item = InitializeExpenseItem(itemViewModel);
				item.ExpenseReportId = report.ExpenseReportId;
				if (itemIds.Contains(item.ExpenseItemId))
				{
					AppService.UpdateExpenseItem(item);
					itemIds.Remove(item.ExpenseItemId);
				}
				else
				{
					AppService.CreateExpenseItem(item);
				}
			}

			foreach (int itemId in itemIds)
			{
				AppService.DeleteExpenseItem(itemId);
			}
		}

		private ExpenseItem InitializeExpenseItem(ExpenseItemCreateViewModel itemViewModel)
		{
			return new ExpenseItem()
			{
				AccountId = itemViewModel.AccountId,
				Amount = itemViewModel.Amount,
				ExpenseItemCreatedUtc = itemViewModel.ExpenseItemCreatedUtc,
				ExpenseItemId = itemViewModel.ExpenseItemId,
				ExpenseItemModifiedUtc = itemViewModel.ExpenseItemModifiedUtc,
				ExpenseReportId = itemViewModel.ExpenseReportId,
				Index = itemViewModel.Index,
				IsBillableToCustomer = itemViewModel.IsBillableToCustomer,
				ItemDescription = itemViewModel.ItemDescription,
				ToDelete = itemViewModel.ToDelete,
				TransactionDate = itemViewModel.TransactionDate
			};
		}

		private void UploadAttachments(ExpenseCreateModel model, ExpenseReport report)
		{
			foreach (string name in AzureFiles.GetReportAttachments(report.ExpenseReportId))
			{
				if (model.PreviousFiles == null || !model.PreviousFiles.Contains(name))
				{
					AzureFiles.DeleteReportAttachment(report.ExpenseReportId, name);
				}
			}

			if (model.Files != null)
			{
				foreach (var file in model.Files)
				{
					if (file != null)
					{
						AzureFiles.SaveReportAttachments(report.ExpenseReportId, file.InputStream, file.FileName);
					}
				}
			}
		}
	}
}