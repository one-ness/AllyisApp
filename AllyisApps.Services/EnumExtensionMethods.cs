namespace AllyisApps.Services.TimeTracker
{
	/// <summary>
	///
	/// </summary>
	public static class EnumExtenstionMethods
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this TimeEntryStatus value)
		{
			string name = "";
			switch (value)
			{
				case TimeEntryStatus.Approved:
					name = "Approved";
					break;

				case TimeEntryStatus.PayrollProcessed:
					name = "Payroll Processed";
					break;

				case TimeEntryStatus.Pending:
					name = "Pending";
					break;

				case TimeEntryStatus.Rejected:
					name = "Rejected";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this PayPeriodType value)
		{
			string name = "";
			switch (value)
			{
				case PayPeriodType.Dates:
					name = "Dates";
					break;

				case PayPeriodType.Duration:
					name = "Duration";
					break;
			}

			return name;
		}
	}
}