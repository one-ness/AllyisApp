namespace AllyisApps.Services.TimeTracker
{
	public enum LockEntriesResult
	{
		InvalidStatuses,
		DBError,
		Success
	}

	public enum UnlockEntriesResult
	{
		NoLockDate,
		DBError,
		Success
	}
}