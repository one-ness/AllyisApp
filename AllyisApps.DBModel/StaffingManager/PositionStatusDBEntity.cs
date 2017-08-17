using System;

namespace AllyisApps.DBModel.StaffingManager
{
	/// <summary>
	/// DB object for position status
	/// </summary>
	public class PositionStatusDBEntity
	{
		/// <summary>
		/// Gets or sets the position status Id
		/// </summary>
		public int PositionStatusId { get; set; }

		/// <summary>
		/// Gets or sets the Position Level's Name
		/// </summary>
		public string PositionStatusName { get; set; }
	}
}
