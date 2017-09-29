namespace AllyisApps.Services.Expense
{
	/// <summary>
	/// Represents the status of a expense report
	/// </summary>
	public enum ExpenseStatusEnum
	{
		/// <summary>
		/// Expense is still in being drafted.
		/// </summary>
		Draft = 0,

		/// <summary>
		/// Expense is pending.
		/// </summary>
		Pending = 1,

		/// <summary>
		/// Expense has been approved
		/// </summary>
		Approved = 2,

		/// <summary>
		/// Expense has been rejected
		/// </summary>
		Rejected = 3,

		/// <summary>
		/// Expense has been paid out.
		/// </summary>
		Paid = 4
	}
}