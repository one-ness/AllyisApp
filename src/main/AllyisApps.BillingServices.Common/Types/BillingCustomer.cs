using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.BillingServices.Common.Types
{
	public class BillingCustomer
	{
		private readonly BillingServicesCustomerId _BillingServicesCustomerId;
		private readonly string _Last4;

		public BillingCustomer(BillingServicesCustomerId customerId, string last4 = "nnnn")
		{
			#region last4 validation
			if (last4.Length != 4)
			{
				throw new ArgumentException("last4", "last 4 must be exactly 4 numbers");
			}

			int num;
			if (last4 != "nnnn" && !(int.TryParse(last4, out num)))
			{
				throw new ArgumentException("last4", "last 4 must be exactly 4 numbers");
			}
			#endregion

			_Last4 = last4;
			_BillingServicesCustomerId = customerId;
		}

		public BillingServicesCustomerId Id
		{
			get
			{
				return _BillingServicesCustomerId;
			}
		}

		public string Last4
		{
			get
			{
				return _Last4;
			}
		}
	}
}
