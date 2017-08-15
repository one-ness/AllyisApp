using System;

namespace AllyisApps.Services.StaffingManager
{
	public enum PositionStatusEnum : int
	{
		/// <summary>
		/// Active.
		/// </summary>
		Active,

		/// <summary>
		/// Closed, no longer accepting applicants.
		/// </summary>
		Closed,

		/// <summary>
		/// Filled, the position has been filled, no longer accpeting apllicants.
		/// </summary>
		Won,

		/// <summary>
		/// On Hold.
		/// </summary>
		OnHold,

		/// <summary>
		/// lost.
		/// </summary>
		Lost
	}
}
