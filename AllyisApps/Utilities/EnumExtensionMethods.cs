using System;
using AllyisApps.Resources;

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
					name = Strings.Hourly;
					break;

				case BillingRateEnum.Monthly:
					name = Strings.Months;
					break;

				case BillingRateEnum.Yearly:
					name = "Yearly";
					break;
			}

			return name;
		}

		public static string GetEnumName(this AppService.OrgAction value)
		{
			string name = "";
			switch (value)
			{
				case AppService.OrgAction.CreateSubscription:
					name = Strings.CreateSubscription;
					break;

				case AppService.OrgAction.CreateBilling:
					name = "Create Billing";
					break;

				case AppService.OrgAction.ReadBilling:
					name = "Read Billing";
					break;

				case AppService.OrgAction.ReadInvitationsList:
					break;

				case AppService.OrgAction.ReadOrganization:
					break;

				case AppService.OrgAction.ReadSubscription:
					break;

				case AppService.OrgAction.ReadPermissions:
					break;

				case AppService.OrgAction.ReadSubscriptions:
					break;

				case AppService.OrgAction.ReadUser:
					break;

				case AppService.OrgAction.ReadUsersList:
					break;

				case AppService.OrgAction.EditUser:
					break;

				case AppService.OrgAction.EditUserPermission:
					break;

				case AppService.OrgAction.EditOrganization:
					break;

				case AppService.OrgAction.EditSubscription:
					break;

				case AppService.OrgAction.EditSubscriptionUser:
					break;

				case AppService.OrgAction.EditBilling:
					break;

				case AppService.OrgAction.AddUserToOrganization:
					break;

				case AppService.OrgAction.AddUserToSubscription:
					break;

				case AppService.OrgAction.ChangePassword:
					break;

				case AppService.OrgAction.ResendInvitation:
					break;

				case AppService.OrgAction.DeleteUserFromOrganization:
					break;

				case AppService.OrgAction.DeleteUserFromSubscription:
					break;

				case AppService.OrgAction.DeleteInvitation:
					break;

				case AppService.OrgAction.DeleteOrganization:
					break;

				case AppService.OrgAction.DeleteSubscritpion:
					break;

				case AppService.OrgAction.DeleteBilling:
					break;
			}
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
					name = Strings.BillingAccepted;
					break;

				case InvitationStatusEnum.Any:
					name = Strings.Any;
					break;

				case InvitationStatusEnum.Pending:
					name = Strings.Pending;
					break;

				case InvitationStatusEnum.Rejected:
					name = Strings.Rejected;
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
					name = Strings.Member;
					break;

				case OrganizationRoleEnum.Owner:
					name = Strings.Owner;
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
					name = Strings.Manager;
					break;

				case StaffingManagerRole.NotInProduct:
					name = Strings.Unassigned;
					break;

				case StaffingManagerRole.User:
					name = Strings.User;
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
					name = Strings.Manager;
					break;

				case TimeTrackerRole.NotInProduct:
					name = Strings.Unassigned;
					break;

				case TimeTrackerRole.User:
					name = Strings.User;
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
					name = Strings.Manager;
					break;

				case ExpenseTrackerRole.NotInProduct:
					name = Strings.Unassigned;
					break;

				case ExpenseTrackerRole.SuperUser:
					name = Strings.SuperUser;
					break;

				case ExpenseTrackerRole.User:
					name = Strings.User;
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
					name = Strings.ExpenseTracker;
					break;

				case ProductIdEnum.None:
					name = Strings.None;
					break;

				case ProductIdEnum.StaffingManager:
					name = Strings.StaffingManager;
					break;

				case ProductIdEnum.TimeTracker:
					name = Strings.TimeTracker;
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
					name = Strings.None;
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
					name = Strings.User;
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
					name = Strings.Any;
					break;

				case ExpenseStatusEnum.Approved:
					name = Strings.Approved;
					break;

				case ExpenseStatusEnum.Draft:
					name = "Draft";
					break;

				case ExpenseStatusEnum.Paid:
					name = "Paid";
					break;

				case ExpenseStatusEnum.Pending:
					name = Strings.Pending;
					break;

				case ExpenseStatusEnum.Rejected:
					name = Strings.Rejected;
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