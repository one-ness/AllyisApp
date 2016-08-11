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
	public class BillingServicesCustomerId
	{
		#region private fields
		private readonly string id;
		#endregion

		#region constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		public BillingServicesCustomerId(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException("id", "id must have a value.");
			}

			this.id = id;
		}
		#endregion

		#region accessor properties
		/// <summary>
		/// 
		/// </summary>
		public string Id
		{
			get
			{
				return this.id;
			}
		}
		#endregion
	}
}
