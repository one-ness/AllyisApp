using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.BillingServices.Common.Types
{
	public class BillingServicesToken
	{
		private readonly string _token;

		public BillingServicesToken(string token)
		{
			_token = token;
		}

		public string Token
		{
			get
			{
				return _token;
			}

		}
	}
}
