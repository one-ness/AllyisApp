using System;

namespace AllyisApps.Services.StaffingManager
{
	public class ApplicationStatus
	{
		private string applicationStatusName;

		/// <summary>
		/// Gets or sets the position status Id
		/// </summary>
		public int ApplicationStatusId { get; set; }

		/// <summary>
		/// Gets or sets the Position Level's Name
		/// </summary>
		public string ApplicationStatusName
		{
			get => applicationStatusName;
			set
			{
				if (value.Length > 32 || value.Length == 0) throw new ArgumentOutOfRangeException(nameof(applicationStatusName), value, nameof(applicationStatusName) + " must be between 1 and 32 characters in length");
				applicationStatusName = value;
			}
		}
	}
}