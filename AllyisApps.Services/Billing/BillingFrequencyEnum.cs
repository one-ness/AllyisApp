#pragma warning disable 1591

namespace AllyisApps.Services
{
	/// <summary>
	/// Billing frequency.
	/// </summary>
	public enum BillingFrequencyEnum : byte
	{
		Monthly = 1,
		Quarterly = 3,
		TriAnnual = 4,
		SemiAnnual = 6,
		Annual = 12,
	}
}
