#pragma warning disable 1591

namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// Block based on. Must match the values inserted in to db.
	/// </summary>
	public enum BlockBasedOnEnum : byte
	{
		PerUser = 1,
		PerExpenseReport = 2,
	}
}