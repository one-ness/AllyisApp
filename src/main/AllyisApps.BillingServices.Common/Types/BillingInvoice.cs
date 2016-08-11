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
	public class BillingInvoice
	{
		#region private fields
		private readonly int amountDue;
		private readonly DateTime? date;
		private readonly string id;
		private readonly string productName;
		private readonly string service;
		#endregion

		#region constructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="amountDue"></param>
		/// <param name="date"></param>
		/// <param name="id"></param>
		/// <param name="productName"></param>
		/// <param name="service"></param>
		public BillingInvoice(int amountDue, DateTime? date, string id, string productName, string service)
		{
			this.amountDue = amountDue;
			this.date = date;
			this.id = id;
			this.productName = productName;
			this.service = service;
		}
		#endregion

		#region accessor properties
		/// <summary>
		/// 
		/// </summary>
		public int AmountDue
		{
			get
			{
				return this.amountDue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public DateTime? Date
		{
			get
			{
				return this.date;
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
		public string ProductName
		{
			get
			{
				return this.productName;
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
