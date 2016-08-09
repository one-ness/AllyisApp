using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.BillingServices.Common.Types
{
	public class BillingInvoice
	{
		#region private fields
		private readonly int _AmountDue;
		private readonly DateTime? _Date;
		private readonly string _Id;
		private readonly string _ProductName;
		private readonly string _Service;
		#endregion

		#region constructor
		public BillingInvoice(int amountDue, DateTime? date, string id, string productName, string service)
		{
			_AmountDue = amountDue;
			_Date = date;
			_Id = id;
			_ProductName = productName;
			_Service = service;
		}
		#endregion

		#region accessor properties
		public int AmountDue
		{
			get
			{
				return _AmountDue;
			}
		}

		public DateTime? Date
		{
			get
			{
				return _Date;
			}
		}

		public string Id
		{
			get
			{
				return _Id;
			}
		}

		public string ProductName
		{
			get
			{
				return _ProductName;
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
