//------------------------------------------------------------------------------
// <copyright file="CreateUpdateTimeEntryResultEnum.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.TimeTracker
{
	public enum CreateUpdateTimeEntryResult
	{
		OvertimePayClass,
		InvalidPayClass,
		InvalidProject,
		ZeroDuration,
		Over24Hours,
		EntryIsLocked,
		Success
	}
}
