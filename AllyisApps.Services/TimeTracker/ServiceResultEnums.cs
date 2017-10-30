namespace AllyisApps.Services.TimeTracker
{
	public enum LockEntriesResult
	{
		InvalidStatuses,
		DBError,
		Success,
		InvalidLockDate
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
}