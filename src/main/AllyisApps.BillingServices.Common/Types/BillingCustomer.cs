using System;

namespace AllyisApps.BillingServices.Common.Types
{
	/// <summary>
	/// 
	/// </summary>
	public class BillingCustomer
	{
		#region private fields
		private readonly BillingServicesCustomerId billingServicesCustomerId;
		private readonly string last4;
		#endregion

		#region constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="last4"></param>
		public BillingCustomer(BillingServicesCustomerId customerId, string last4 = "nnnn")
		{
			#region last4 validation
			if (last4.Length != 4)
			{
				throw new ArgumentException("last4", "last 4 must be exactly 4 numbers");
			}

			int num;
			if (last4 != "nnnn" && !int.TryParse(last4, out num))
			{
				throw new ArgumentException("last4", "last 4 must be exactly 4 numbers");
			}
			#endregion

			this.last4 = last4;
			this.billingServicesCustomerId = customerId;
		}
		#endregion

		#region accessor properties
		/// <summary>
		/// 
		/// </summary>
		public BillingServicesCustomerId Id
		{
			get
			{
				return this.billingServicesCustomerId;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Last4
		{
			get
			{
				return this.last4;
			}
		}
		#endregion
	}
}
