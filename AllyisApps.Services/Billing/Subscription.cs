using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// a subscription (by an organization to a sku of a product)
	/// </summary>
	public class Subscription
	{
		public int SubscriptionId { get; set; }
		public int OrganizationId { get; set; }
		public SkuIdEnum SkuId { get; set; }
		public string SubscriptionName { get; set; }
		public int NumberOfUsers { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreatedUtc { get; set; }
		public DateTime? PromoExpirationDateUtc { get; set; }
		public string SkuName { get; set; }
		public ProductIdEnum ProductId { get; set; }
		public string ProductName { get; set; }
		public string AreaUrl { get; set; }
	}
}