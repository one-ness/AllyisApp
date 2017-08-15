using System;

namespace AllyisApps.Services.TimeTracker
{
	public enum EmploymentTypeEnum : int
	{
		/// <summary>
		/// Hourly Pay.
		/// </summary>
		Hourly,

		/// <summary>
		/// Salaried Pay.
		/// </summary>
		Salary,

		/// <summary>
		/// Part-Time Hourly pay.
		/// </summary>
		PartTimeHourly,

		/// <summary>
		/// Part Time Salaried pay.
		/// </summary>
		PartTimeSalary,
	}
}
