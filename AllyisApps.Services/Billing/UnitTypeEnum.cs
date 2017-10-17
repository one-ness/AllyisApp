namespace AllyisApps.Services.Billing
{
	/// <summary>
	/// unit type for billing
	/// </summary>
	public enum UnitTypeEnum : byte
	{
		// time tracker
		User = 10,

		// expense tracker
		ExpenseReport = 20,

		// staffing manager
		PositionOrCandidate = 30,
	}
}
