using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.BillingServices.Common.Types
{
	/// <summary>
	/// 
	/// </summary>
	public class BillingServicesToken
	{
		#region private fields
		private readonly string token;
		#endregion

		#region constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="token"></param>
		public BillingServicesToken(string token)
		{
			this.token = token;
		}
		#endregion

		#region accessor properties
		/// <summary>
		/// 
		/// </summary>
		public string Token
		{
			get
			{
				return this.token;
			}
		}
		#endregion
	}
}
