//------------------------------------------------------------------------------
// <copyright file="SettingDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.TimeTracker
{
	/// <summary>
	/// The POCO for the Settings table.
	/// </summary>
	public class SettingDBEntity
	{
		/// <summary>
		/// Gets or sets the OrganizationId.
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the StartOfWeek.
		/// </summary>
		public int StartOfWeek { get; set; }

		/// <summary>
		/// Gets or sets the OvertimeHours.
		/// </summary>
		public int OvertimeHours { get; set; }

		/// <summary>
		/// Gets or sets the OvertimePeriod.
		/// </summary>
		public string OvertimePeriod { get; set; }

		/// <summary>
		/// Gets or sets the OvertimeMultiplier.
		/// </summary>
		public decimal OvertimeMultiplier { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to use a lock date.
		/// </summary>
		public bool LockDateUsed { get; set; }

		/// <summary>
		/// Gets or sets the LockDatePeriod.
		/// </summary>
		public string LockDatePeriod { get; set; }

		/// <summary>
		/// Gets or sets the LockDateQuantity.
		/// </summary>
		public int LockDateQuantity { get; set; }
	}
}
