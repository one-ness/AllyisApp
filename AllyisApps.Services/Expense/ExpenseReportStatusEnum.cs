namespace AllyisApps.Services.Expense
{
	/// <summary>
	/// Represents the status of a expense report
	/// </summary>
	public enum ExpenseStatusEnum
	{
		/// <summary>
		/// any
		/// </summary>
		Any = 65535,

		/// <summary>
		/// Expense is still in being drafted.
		/// </summary>
		Draft = 1,

		/// <summary>
		/// Expense is pending.
		/// </summary>
		Pending = 2,

		/// <summary>
		/// Expense has been approved
		/// </summary>
		Approved = 4,

		/// <summary>
		/// Expense has been rejected
		/// </summary>
		Rejected = 8,

		/// <summary>
		/// Expense has been paid out.
		/// </summary>
		Paid = 16
	}
}