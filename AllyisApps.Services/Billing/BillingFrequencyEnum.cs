#pragma warning disable 1591

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// Billing frequency.
	/// </summary>
	public enum BillingFrequencyEnum : byte
	{
		Monthly = 1,
		Quarterly = 2,
		TriAnnual = 4,
		SemiAnnual = 8,
		Annual = 16,
	}
}