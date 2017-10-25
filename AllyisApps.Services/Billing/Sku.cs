namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// sku
	/// </summary>
	public class Sku
	{
		public SkuIdEnum SkuId { get; set; }
		public ProductIdEnum ProductId { get; set; }
		public string SkuName { get; set; }
		public int UserLimit { get; set; }
		public decimal CostPerUnit { get; set; }
		public UnitTypeEnum UnitType { get; set; }
		public BillingFrequencyEnum BillingFrequency { get; set; }
		public int UnitSize { get; set; }
		public bool IsActive { get; set; }
		public string SkuDescription { get; set; }
		public decimal? PromotionalCostPerUnit { get; set; }
		public int PromotionDurationDays { get; set; }
		public string IconUrl { get; set; }
	}
}
