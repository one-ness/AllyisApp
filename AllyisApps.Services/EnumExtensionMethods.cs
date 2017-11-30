using System;

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
			switch (value)
			{
				case TimeEntryStatus.Approved:
					return "Approved";
				case TimeEntryStatus.PayrollProcessed:
					return "Payroll Processed";
				case TimeEntryStatus.Pending:
					return "Pending";
				case TimeEntryStatus.Rejected:
					return "Rejected";
				default:
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this PayPeriodType value)
		{
			switch (value)
			{
				case PayPeriodType.Dates:
					return "Dates";
				case PayPeriodType.Duration:
					return "Duration";
				default:
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
		}
	}
}