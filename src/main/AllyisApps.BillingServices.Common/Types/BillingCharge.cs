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
	public class BillingCharge
	{
		#region private fields
		private readonly int amount;
		private readonly DateTime created;
		private readonly string id;
		private readonly string statementDescriptor;
		private readonly string service;
		#endregion

		#region constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="created"></param>
		/// <param name="id"></param>
		/// <param name="statementDescriptor"></param>
		/// <param name="service"></param>
		public BillingCharge(int amount, DateTime created, string id, string statementDescriptor, string service = "Stripe")
		{
			this.amount = amount;
			this.created = created;
			this.id = id;
			this.statementDescriptor = statementDescriptor;
			this.service = service;
		}
		#endregion

		#region accessor properties
		/// <summary>
		/// 
		/// </summary>
		public int Amount
		{
			get
			{
				return this.amount;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public DateTime Created
		{
			get
			{
				return this.created;
			}
		}

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

		/// <summary>
		/// 
		/// </summary>
		public string StatementDescriptor
		{
			get
			{
				return this.statementDescriptor;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Service
		{
			get
			{
				return this.service;
			}
		}
		#endregion
	}
}
