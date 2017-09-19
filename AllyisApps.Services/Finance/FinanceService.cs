using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Finance;
using AllyisApps.DBModel.Lookup;
using AllyisApps.Lib;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Lookup;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all finance related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		public IList<Account> GetAccounts()
		{
			return DBHelper.GetAccounts().Select(x => InitializeAccountModel(x)).ToList();
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

		public void DeleteAccount(int id)
		{
			DBHelper.DeleteAccount(id);
			return;
		}

		public Account InitializeAccountModel(AccountDBEntity account)
		{
			return new Account()
			{
				AccountId = account.AccountId,
				AccountName = account.AccountName,
				AccountTypeId = account.AccountTypeId,
				AccountTypeName = account.AccountTypeName,
				IsActive = account.IsActive,
				ParentAccountId = account.ParentAccountId != null ? account.ParentAccountId : null
			};
		}

		public AccountDBEntity InitializeAccountDbModel(Account account)
		{
			return new AccountDBEntity()
			{
				AccountId = account.AccountId,
				AccountName = account.AccountName,
				AccountTypeId = account.AccountTypeId,
				AccountTypeName = account.AccountTypeName,
				IsActive = account.IsActive,
				ParentAccountId = account.ParentAccountId
			};
		}
	}
}