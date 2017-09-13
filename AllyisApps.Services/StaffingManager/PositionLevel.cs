﻿using System;

namespace AllyisApps.Services.StaffingManager
{
	public class PositionLevel
	{
		private string positionLevelName;

		/// <summary>
		/// Gets or sets the position level Id
		/// </summary>
		public int PositionLevelId { get; set; }

		/// <summary>
		/// Gets or sets the Position Level's Name
		/// </summary>
		public string PositionLevelName
		{
			get => positionLevelName;
			set
			{
				if (value.Length > 32 || value.Length == 0) throw new ArgumentOutOfRangeException(nameof(positionLevelName), value, nameof(positionLevelName) + " must be between 1 and 32 characters in length");
				positionLevelName = value;
			}
		}
	}
}