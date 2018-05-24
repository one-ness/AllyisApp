using System;
using AllyisApps.Resources;
using AllyisApps.Services.Hrm;

#region service

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

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
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
					name = "Read Invitations List";
					break;

				case AppService.OrgAction.ReadOrganization:
					name = "Read Organization";
					break;

				case AppService.OrgAction.ReadSubscription:
					name = "Read Subscription";
					break;

				case AppService.OrgAction.ReadPermissions:
					name = "Read Permissions";
					break;

				case AppService.OrgAction.ReadSubscriptions:
					name = "Read Subscriptions";
					break;

				case AppService.OrgAction.ReadUser:
					name = "Read User";
					break;

				case AppService.OrgAction.ReadUsersList:
					name = "ReadUsersList";
					break;

				case AppService.OrgAction.EditUser:
					name = Strings.EditUser;
					break;

				case AppService.OrgAction.EditUserPermission:
					name = "Edit User Permission";
					break;

				case AppService.OrgAction.EditOrganization:
					name = Strings.EditOrganization;
					break;

				case AppService.OrgAction.EditSubscription:
					name = Strings.EditSubscription;
					break;

				case AppService.OrgAction.EditSubscriptionUser:
					name = "Edit Subscription User";
					break;

				case AppService.OrgAction.EditBilling:
					name = "Edit Billing";
					break;

				case AppService.OrgAction.AddUserToOrganization:
					name = "Add User To Organization";
					break;

				case AppService.OrgAction.AddUserToSubscription:
					name = "Add User To Subscription";
					break;

				case AppService.OrgAction.ChangePassword:
					name = Strings.ChangePassword;
					break;

				case AppService.OrgAction.ResendInvitation:
					name = Strings.ResendInvite;
					break;

				case AppService.OrgAction.DeleteUserFromOrganization:
					name = "Delete User From Organization";
					break;

				case AppService.OrgAction.DeleteUserFromSubscription:
					name = "Delete User From Subscription";
					break;

				case AppService.OrgAction.DeleteInvitation:
					name = "Delete Invitation";
					break;

				case AppService.OrgAction.DeleteOrganization:
					name = Strings.DeleteOrganization;
					break;

				case AppService.OrgAction.DeleteSubscritpion:
					name = "Delete Subscription";
					break;

				case AppService.OrgAction.DeleteBilling:
					name = "Delete Billing";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this AppService.TimeTrackerAction value)
		{
			var name = "";

			switch (value)
			{
				case AppService.TimeTrackerAction.TimeEntry:
					name = Strings.TimeEntry;
					break;

				case AppService.TimeTrackerAction.EditCustomer:
					name = Strings.EditCustomer;
					break;

				case AppService.TimeTrackerAction.ViewOthers:
					name = "View Others";
					break;

				case AppService.TimeTrackerAction.ViewCustomer:
					name = "View Customer";
					break;

				case AppService.TimeTrackerAction.EditProject:
					name = Strings.EditProject;
					break;

				case AppService.TimeTrackerAction.EditOthers:
					name = "Edit Others";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this AppService.ExpenseTrackerAction value)
		{
			var name = "";

			switch (value)
			{
				case AppService.ExpenseTrackerAction.Unmanaged:
					name = "Unmanaged";
					break;

				case AppService.ExpenseTrackerAction.EditReport:
					name = "Edit Report";
					break;

				case AppService.ExpenseTrackerAction.AdminReport:
					name = "Admin Report";
					break;

				case AppService.ExpenseTrackerAction.AdminExpense:
					name = "Admin Expense";
					break;

				case AppService.ExpenseTrackerAction.StatusUpdate:
					name = "Status Update";
					break;

				case AppService.ExpenseTrackerAction.Pending:
					name = Strings.Pending;
					break;

				case AppService.ExpenseTrackerAction.UpdateReport:
					name = "Update Report";
					break;

				case AppService.ExpenseTrackerAction.CreateReport:
					name = "Create Report";
					break;

				case AppService.ExpenseTrackerAction.UserSettings:
					name = "User Settings";
					break;

				case AppService.ExpenseTrackerAction.Accounts:
					name = "Accounts";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this AppService.StaffingAction value)
		{
			var name = "";

			switch (value)
			{
				case AppService.StaffingAction.Index:
					name = Strings.Index;
					break;

				case AppService.StaffingAction.EditPosition:
					name = "Edit Positions";
					break;

				case AppService.StaffingAction.ViewPosition:
					name = "View Position";
					break;

				case AppService.StaffingAction.EditApplicant:
					name = "Edit Applicant";
					break;

				case AppService.StaffingAction.ViewApplicant:
					name = "View Applicant";
					break;

				case AppService.StaffingAction.EditApplication:
					name = "Edit Application";
					break;

				case AppService.StaffingAction.ViewApplication:
					name = "View Application";
					break;

				case AppService.StaffingAction.ViewOthers:
					name = "View Others";
					break;

				case AppService.StaffingAction.EditOthers:
					name = "Edit Other";
					break;
			}

			return name;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this AppService.StaffingManagerAction value)
		{
			var name = "";

			switch (value)
			{
				case AppService.StaffingManagerAction.EditCustomer:
					name = Strings.EditCustomer;
					break;
			}

			return name;
		}
	}
}

#endregion service

#region auth

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

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetEnumName(this UpdateEmployeeIdAndOrgRoleResult value)
		{
			var name = "";

			switch (value)
			{
				case UpdateEmployeeIdAndOrgRoleResult.Success:
					name = "Success";
					break;

				case UpdateEmployeeIdAndOrgRoleResult.CannotSelfUpdateOrgRole:
					name = "Cannot Self Update OrgRole";
					break;

				case UpdateEmployeeIdAndOrgRoleResult.EmployeeIdNotUnique:
					name = "EmployeeId Not Unique";
					break;
			}

			return name;
		}
	}
}

#endregion auth

#region billing

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

#endregion billing

#region expense

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

#endregion expense

#region timetracker

namespace AllyisApps.Services.TimeTracker
{
	/// <summary>
	///
	/// </summary>
	public static class EnumExtenstionMethods
	{
		/// <summary>
		/// get the enum name
		/// </summary>
		public static string GetEnumName(this Services.Hrm.BuiltinPayClassEnum value)
		{
			string name = "";
			switch (value)
			{
				case BuiltinPayClassEnum.Holiday:
					name = "Holiday";
					break;

				case BuiltinPayClassEnum.PaidTimeOff:
					name = "Paid Time Off";
					break;

				case BuiltinPayClassEnum.Regular:
					name = "Regular";
					break;

				case BuiltinPayClassEnum.Overtime:
					name = "Overtime";
					break;

				case BuiltinPayClassEnum.UnpaidTimeOff:
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
		/// Gets the corresponding localized result message based on the result enum.
		/// </summary>
		/// <param name="value">The result enum</param>
		/// <param name="modelDate">Date of the time entry being created/updated.</param>
		/// <returns>Localized result message based on the result enum.</returns>
		public static string GetResultMessage(this CreateUpdateTimeEntryResult value, DateTime modelDate)
		{
			switch (value)
			{
				case CreateUpdateTimeEntryResult.OvertimePayClass:
					return Strings.TimeEntryResultOvertimePayClass;
				case CreateUpdateTimeEntryResult.InvalidPayClass:
					return string.Format(Strings.MustSelectPayClass, modelDate.ToShortDateString());
				case CreateUpdateTimeEntryResult.InvalidProject:
					return string.Format(Strings.MustSelectProject, modelDate.ToShortDateString());
				case CreateUpdateTimeEntryResult.ZeroDuration:
					return string.Format(Strings.EnterATimeLongerThanZero, modelDate.ToShortDateString());
				case CreateUpdateTimeEntryResult.Over24Hours:
					return string.Format(Strings.CannotExceed24, modelDate.ToShortDateString());
				case CreateUpdateTimeEntryResult.EntryIsLocked:
					return Strings.TimeEntryResultLocked;
				case CreateUpdateTimeEntryResult.Success:
					return string.Empty;
				case CreateUpdateTimeEntryResult.MustBeAssignedToProject:
					return Strings.MustBeAssignedToProject;
				case CreateUpdateTimeEntryResult.ProjectIsNotActive:
					return Strings.ProjectIsNotActive;
				case CreateUpdateTimeEntryResult.NotAuthZTimeEntryOtherUserEdit:
					return Strings.NotAuthZTimeEntryOtherUserEdit;
				case CreateUpdateTimeEntryResult.InvalidAction:
					return string.Format(Strings.TimeEntryResultInvalidAction, modelDate.ToShortDateString());
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}

#endregion timetracker

#region timetracker.core

namespace AllyisApps.Areas.TimeTracker.Core
{
	/// <summary>
	///
	/// </summary>
	public static class EnumExtenstionMethods
	{
		/// <summary>
		/// Gets the name of the specified enum value.
		/// </summary>
		/// <param name="value">The specified enum</param>
		public static string GetEnumName(this ApprovalState value)
		{
			var name = "";

			switch (value)
			{
				case ApprovalState.Approved:
					name = Strings.Approved;
					break;

				case ApprovalState.NoApprovalState:
					name = "Not Approval State";
					break;

				case ApprovalState.NotApproved:
					name = "Not Approved";
					break;
			}

			return name;
		}
	}
}

#endregion timetracker.core

#region staffingmanager

namespace AllyisApps.Services.StaffingManager
{
	/// <summary>
	///
	/// </summary>
	public static class EnumExtenstionMethods
	{
		/// <summary>
		/// Gets the name of the specified enum value.
		/// </summary>
		/// <param name="value">The specified enum</param>
		public static string GetEnumName(this ApplicationStatusEnum value)
		{
			var name = "";

			switch (value)
			{
				case ApplicationStatusEnum.Accepted:
					name = Strings.Accept;
					break;

				case ApplicationStatusEnum.InProgress:
					name = "In Progress";
					break;

				case ApplicationStatusEnum.Rejected:
					name = Strings.Rejected;
					break;
			}

			return name;
		}
	}
}

#endregion staffingmanager