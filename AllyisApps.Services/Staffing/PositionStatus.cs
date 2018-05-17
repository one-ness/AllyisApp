using System;

namespace AllyisApps.Services.StaffingManager
{
	public class PositionStatus
	{
		private string positionStatusName;

		/// <summary>
		/// Gets or sets the position status Id
		/// </summary>
		public int PositionStatusId { get; set; }

		/// <summary>
		/// Gets or sets the Position Level's Name
		/// </summary>
		public string PositionStatusName
		{
			get => positionStatusName;
			set
			{
				if (value.Length > 32 || value.Length == 0) throw new ArgumentOutOfRangeException(nameof(positionStatusName), value, nameof(positionStatusName) + " must be between 1 and 32 characters in length");
				positionStatusName = value;
			}
		}
	}
}