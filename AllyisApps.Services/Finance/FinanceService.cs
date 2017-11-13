using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllyisApps.DBModel.Finance;
using AllyisApps.Services.Expense;
using AllyisApps.Services.Auth;
using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all finance related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		public async Task<IList<Account>> GetAccounts(int orgId)
		{
			var results = await DBHelper.GetAccounts(orgId);
			return results.Select(x => InitializeAccountModel(x)).ToList();
		}

		public bool UpdateAccount(Account item)
		{
			AccountDBEntity entity = InitializeAccountDbModel(item);
			var result = DBHelper.UpdateAccount(entity);
			return result != -1 ? true : false;
		}

		public bool CreateAccount(Account item)
		{
			AccountDBEntity entity = InitializeAccountDbModel(item);
			var result = DBHelper.CreateAccount(entity);
			return result != -1 ? true : false;
		}

		public async Task DeleteAccount(int id)
		{
			await DBHelper.DeleteAccount(id);
			return;
		}

		public Account InitializeAccountModel(AccountDBEntity account)
		{
			return new Account
			{
				AccountId = account.AccountId,
				AccountName = account.AccountName,
				OrganizationId = account.OrganizationId,
				AccountTypeId = account.AccountTypeId,
				AccountTypeName = account.AccountTypeName,
				IsActive = account.IsActive,
				ParentAccountId = account.ParentAccountId != null ? account.ParentAccountId : null
			};
		}

		public AccountDBEntity InitializeAccountDbModel(Account account)
		{
			return new AccountDBEntity
			{
				AccountId = account.AccountId,
				AccountName = account.AccountName,
				OrganizationId = account.OrganizationId,
				AccountTypeId = account.AccountTypeId,
				AccountTypeName = account.AccountTypeName,
				IsActive = account.IsActive,
				ParentAccountId = account.ParentAccountId
			};
		}




		/// <summary>
		/// Checks if the supplied account can be deleted.
		/// </summary>
		/// <returns></returns>
		public async Task<CanDeleteResults> CanDelete(int subId, int accId)
		{
			var subInfo = UserContext.SubscriptionsAndRoles[subId];
			List<Account> accounts = GetAccounts(subInfo.OrganizationId).Result.ToList();

			List<ExpenseReport> reports = GetExpenseReportByOrgId(subInfo.OrganizationId).Result.ToList();
			List<ExpenseItem> items = new List<ExpenseItem>();

			List<Account> associatedAccounts = new List<Account>();

			//Get all report items to check their associated account.
			foreach (var report in reports)
			{
				items.AddRange(await GetExpenseItemsByReportId(report.ExpenseReportId));
			}
			List<Account> parentAccounts = accounts.Where(x => x.AccountId == accId).ToList();

			while (parentAccounts.Count > 0)
			{
				List<Account> nextAccounts = new List<Account>();

				foreach (var parent in parentAccounts)
				{
					var currentAccItem = items.Where(x => x.AccountId == parent.AccountId);

					if (currentAccItem.Count() > 0)
					{
						return new CanDeleteResults
						{
							canDelete = false,
							associatedAccounts = associatedAccounts
						};
					}

					nextAccounts.AddRange(accounts.Where(x => x.ParentAccountId == parent.AccountId));
				}
				associatedAccounts.AddRange(parentAccounts);
				parentAccounts = nextAccounts;
			}

			//No reports means we can remove the account(s).
			if (items.Count == 0)
			{
				return new CanDeleteResults
				{
					canDelete = false,
					associatedAccounts = associatedAccounts
				};
			}

			return new CanDeleteResults
			{
				canDelete = true,
				associatedAccounts = associatedAccounts
			}; ;
		}
	}

	public class CanDeleteResults
	{
		public bool canDelete { get; set; }
		public List<Account> associatedAccounts { get; set; }
	}
}