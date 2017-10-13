using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// sku
	/// </summary>
	public class Sku
	{
		public SkuIdEnum SkuId { get; set; }
		public ProductIdEnum ProductIdEnum { get; set; }
		public string SkuName { get; set; }
		public int UserLimit { get; set; }
		public byte BillingFrequency { get; set; }
		public bool IsActive { get; set; }
		public string SkuDescription { get; set; }
		public string IconUrl { get; set; }
	}
}
