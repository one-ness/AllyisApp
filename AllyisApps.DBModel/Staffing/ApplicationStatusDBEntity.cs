using System;

namespace AllyisApps.DBModel.Staffing
{
	/// <summary>
	/// DB object for position status
	/// </summary>
	public class ApplicationStatusDBEntity
	{
		private string applicationStatusName;

		/// <summary>
		/// Gets or sets the application status Id
		/// </summary>
		public int ApplicationStatusId { get; set; }

		/// <summary>
		/// Gets or sets Organization Id
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets the applications status's Name
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