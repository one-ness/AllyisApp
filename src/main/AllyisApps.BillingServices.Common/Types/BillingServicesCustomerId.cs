using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.BillingServices.Common.Types
{
	public class BillingServicesCustomerId
	{
		private readonly string _ID;

		public BillingServicesCustomerId(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException("id", "id must have a value.");
			}

			_ID = id;
		}

		public string Id
		{
			get
			{
				return _ID;
			}

		}
	}
}
