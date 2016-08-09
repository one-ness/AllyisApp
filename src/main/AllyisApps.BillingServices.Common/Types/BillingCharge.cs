using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.BillingServices.Common.Types
{
	public class BillingCharge
	{
		#region private fields
		private readonly int _Amount;
		private readonly DateTime _Created;
		private readonly string _Id;
		private readonly string _StatementDescriptor;
		#endregion

		#region constructor
		public BillingCharge(int amount, DateTime created, string id, string statementDescriptor)
		{
			_Amount = amount;
			_Created = created;
			_Id = id;
			_StatementDescriptor = statementDescriptor;
		}
		#endregion

		#region accessor properties
		public int Amount
		{
			get
			{
				return _Amount;
			}
		}

		public DateTime Created
		{
			get
			{
				return _Created;
			}
		}

		public string Id
		{
			get
			{
				return _Id;
			}
		}

		public string StatementDescriptor
		{
			get
			{
				return _StatementDescriptor;
			}
		}
		#endregion
	}
}
