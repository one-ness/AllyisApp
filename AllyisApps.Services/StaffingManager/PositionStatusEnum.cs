using System;

namespace AllyisApps.Services.TimeTracker
{
	public enum PositionStatusEnum : int
	{
		/// <summary>
		/// Active.
		/// </summary>
		active = 1,

		/// <summary>
		/// Closed, no longer accepting applicants.
		/// </summary>
		closed = 2,

		/// <summary>
		/// Filled, the position has been filled, no longer accpeting apllicants.
		/// </summary>
		won = 3,

		/// <summary>
		/// On Hold.
		/// </summary>
		onHold = 4,

		/// <summary>
		/// lost.
		/// </summary>
		lost = 5
	}
}
