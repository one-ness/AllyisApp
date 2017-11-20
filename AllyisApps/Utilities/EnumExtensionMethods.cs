using System;

namespace AllyisApps.Services
{
	/// <summary>
	/// Contains extension methods for various enumerables.
	/// </summary>
	public static class EnumExtensionMethods
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this BillingRateEnum value)
		{
			string name = "";
			switch (value)
			{
				case BillingRateEnum.Hourly:
					name = "Hourly";
					break;

				case BillingRateEnum.Monthly:
					name = "Monthly";
					break;

				case BillingRateEnum.Yearly:
					name = "Yearly:";
					break;
			}

			return name;
		}
	}
}

namespace AllyisApps.Services.Auth
{
	/// <summary>
	///
	/// </summary>
	public static class EnumExtensionMethods
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this InvitationStatusEnum value)
		{
			string name = "";
			switch (value)
			{
				case InvitationStatusEnum.Accepted:
					name = "Accepted";
					break;

				case InvitationStatusEnum.Any:
					name = "Any";
					break;

				case InvitationStatusEnum.Pending:
					name = "Pending";
					break;

				case InvitationStatusEnum.Rejected:
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
		public static string GetEnumName(this OrganizationRoleEnum value)
		{
			string name = "";
			switch (value)
			{
				case OrganizationRoleEnum.Member:
					name = "Member";
					break;

				case OrganizationRoleEnum.Owner:
					name = "Owner";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this StaffingManagerRole value)
		{
			string name = "";
			switch (value)
			{
				case StaffingManagerRole.Manager:
					name = "Manager";
					break;

				case StaffingManagerRole.NotInProduct:
					name = "Not In Product";
					break;

				case StaffingManagerRole.User:
					name = "User";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this TimeTrackerRole value)
		{
			string name = "";
			switch (value)
			{
				case TimeTrackerRole.Manager:
					name = "Manager";
					break;

				case TimeTrackerRole.NotInProduct:
					name = "Not In Product";
					break;

				case TimeTrackerRole.User:
					name = "User";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this ExpenseTrackerRole value)
		{
			string name = "";
			switch (value)
			{
				case ExpenseTrackerRole.Manager:
					name = "Manager";
					break;

				case ExpenseTrackerRole.NotInProduct:
					name = "Not In Product";
					break;

				case ExpenseTrackerRole.SuperUser:
					name = "Super User";
					break;

				case ExpenseTrackerRole.User:
					name = "User";
					break;
			}

			return name;
		}
	}
}

namespace AllyisApps.Services.Billing
{
	/// <summary>
	///
	/// </summary>
	public static class EnumExtensionMethods
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this BillingFrequencyEnum value)
		{
			string name = "";
			switch (value)
			{
				case BillingFrequencyEnum.Monthly:
					name = "Monthly";
					break;

				case BillingFrequencyEnum.Quarterly:
					name = "Quarterly";
					break;

				case BillingFrequencyEnum.TriAnnual:
					name = "Tri-Annual";
					break;

				case BillingFrequencyEnum.SemiAnnual:
					name = "Semi-Annual";
					break;

				case BillingFrequencyEnum.Annual:
					name = "Annual";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this BillingServicesEnum value)
		{
			string name = "";
			switch (value)
			{
				case BillingServicesEnum.Stripe:
					name = "Stripe";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this ProductIdEnum value)
		{
			string name = "";
			switch (value)
			{
				case ProductIdEnum.AllyisApps:
					name = "Allyis Apps";
					break;

				case ProductIdEnum.ExpenseTracker:
					name = "Expense Tracker";
					break;

				case ProductIdEnum.None:
					name = "Name";
					break;

				case ProductIdEnum.StaffingManager:
					name = "Staffing Manager";
					break;

				case ProductIdEnum.TimeTracker:
					name = "Time Tracker";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this SkuIdEnum value)
		{
			string name = "";
			switch (value)
			{
				case SkuIdEnum.ExpenseTrackerBasic:
					name = "ExpenseTracker Basic";
					break;

				case SkuIdEnum.ExpenseTrackerPro:
					name = "ExpenseTracker Pro";
					break;

				case SkuIdEnum.None:
					name = "None";
					break;

				case SkuIdEnum.StaffingManagerBasic:
					name = "Staffing Manager Basic";
					break;

				case SkuIdEnum.StaffingManagerPro:
					name = "Staffing Manager Pro";
					break;

				case SkuIdEnum.TimeTrackerBasic:
					name = "TimeTracker Basic";
					break;

				case SkuIdEnum.TimeTrackerPro:
					name = "TimeTracker Pro";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this UnitTypeEnum value)
		{
			string name = "";
			switch (value)
			{
				case UnitTypeEnum.ExpenseReport:
					name = "Expense Report";
					break;

				case UnitTypeEnum.PositionOrCandidate:
					name = "Position or Candidate";
					break;

				case UnitTypeEnum.User:
					name = "User";
					break;
			}

			return name;
		}
	}
}

namespace AllyisApps.Services.Expense
{
	/// <summary>
	///
	/// </summary>
	public static class EnumExtensionMethods
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this ExpenseStatusEnum value)
		{
			string name = "";
			switch (value)
			{
				case ExpenseStatusEnum.Any:
					name = "Any";
					break;

				case ExpenseStatusEnum.Approved:
					name = "Approved";
					break;

				case ExpenseStatusEnum.Draft:
					name = "Draft";
					break;

				case ExpenseStatusEnum.Paid:
					name = "Paid";
					break;

				case ExpenseStatusEnum.Pending:
					name = "Pending";
					break;

				case ExpenseStatusEnum.Rejected:
					name = "Rejected";
					break;
			}

			return name;
		}
	}
}

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
		public static string GetEnumName(this PayClassId value)
		{
			string name = "";
			switch (value)
			{
				case PayClassId.Holiday:
					name = "Holiday";
					break;

				case PayClassId.PaidTimeOff:
					name = "Paid Time Off";
					break;

				case PayClassId.Regular:
					name = "Regular";
					break;

				case PayClassId.OverTime:
					name = "Overtime";
					break;

				case PayClassId.UnpaidTimeOff:
					name = "Unpaid Time Off";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this LockEntriesResult value)
		{
			string name = "";
			switch (value)
			{
				case LockEntriesResult.DBError:
					name = "Database Error";
					break;

				case LockEntriesResult.InvalidLockDate:
					name = "Invalid Lock Date";
					break;

				case LockEntriesResult.InvalidStatuses:
					name = "Invalid Statuses";
					break;

				case LockEntriesResult.Success:
					name = "Success";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this UnlockEntriesResult value)
		{
			string name = "";
			switch (value)
			{
				case UnlockEntriesResult.DBError:
					name = "Database Error";
					break;

				case UnlockEntriesResult.NoLockDate:
					name = "No Lock Date";
					break;

				case UnlockEntriesResult.Success:
					name = "Success";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this PayrollProcessEntriesResult value)
		{
			string name = "";
			switch (value)
			{
				case PayrollProcessEntriesResult.DBError:
					name = "Database Error";
					break;

				case PayrollProcessEntriesResult.InvalidStatuses:
					name = "Invalid Statuses";
					break;

				case PayrollProcessEntriesResult.NoLockDate:
					name = "No Lock Date";
					break;

				case PayrollProcessEntriesResult.Success:
					name = "Success";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this StartOfWeekEnum value)
		{
			string name = "";
			switch (value)
			{
				case StartOfWeekEnum.Monday:
					name = "Monday";
					break;

				case StartOfWeekEnum.Tuesday:
					name = "Tuesday";
					break;

				case StartOfWeekEnum.Wednesday:
					name = "Wednesday";
					break;

				case StartOfWeekEnum.Thursday:
					name = "Thursday";
					break;

				case StartOfWeekEnum.Friday:
					name = "Friday";
					break;

				case StartOfWeekEnum.Saturday:
					name = "Saturday";
					break;

				case StartOfWeekEnum.Sunday:
					name = "Sunday";
					break;
			}

			return name;
		}

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