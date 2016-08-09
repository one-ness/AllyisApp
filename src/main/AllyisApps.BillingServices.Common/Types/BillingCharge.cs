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
		private readonly string _Service;
		#endregion

		#region constructor
		public BillingCharge(int amount, DateTime created, string id, string statementDescriptor, string service = "Stripe")
		{
			_Amount = amount;
			_Created = created;
			_Id = id;
			_StatementDescriptor = statementDescriptor;
			_Service = service;
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

		public string Service
		{
			get
			{
				return _Service;
			}
		}
		#endregion
	}
}
