using System;

namespace AllyisApps.Services.TimeTracker
{
	public class StartOfWeekResultClass
	{
		public StartOfWeekResultClass(StartOfWeekResult newEnum)
		{
			Enum = newEnum;
		}

		public StartOfWeekResultClass(StartOfWeekResult newEnum, DateTime suggestedValue)
		{
			Enum = newEnum;
			SuggestedLockDate = suggestedValue;
		}

		public StartOfWeekResult Enum { get; set; }
		public DateTime SuggestedLockDate { get; set; }
	}

	public class OvertimeResultClass
	{
		public OvertimeResultClass(OvertimeResult newEnum)
		{
			Enum = newEnum;
		}

		public OvertimeResultClass(OvertimeResult newEnum, DateTime suggestedValue)
		{
			Enum = newEnum;
			SuggestedLockDate = suggestedValue;
		}

		public OvertimeResult Enum { get; set; }
		public DateTime SuggestedLockDate { get; set; }
	}
}
