namespace AllyisApps.Services.TimeTracker
{
	public enum LockEntriesResult
	{
		InvalidStatuses,
		DBError,
		Success,
		InvalidLockDate,
		NoChange
	}

	public enum UnlockEntriesResult
	{
		NoLockDate,
		DBError,
		Success
	}

	public enum PayrollProcessEntriesResult
	{
		NoLockDate,
		DBError,
		Success,
		InvalidStatuses
	}

	public enum StartOfWeekResult
	{
		StartOfWeekOutOfRange,
		SuccessAndRecalculatedOvertime,
		Success,
		SettingsNotFound,
		InvalidLockDate
	}

	public enum OvertimeResult
	{
		InvalidPeriodValue,
		Success,
		NoHoursValue,
		InvalidHours,
		SettingsNotFound,
		SuccessAndRecalculatedOvertime,
		SuccessAndDeletedOvertime,
		InvalidLockDate
	}
}