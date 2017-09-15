using System;

namespace AllyisApps.Services.StaffingManager
{
	public class EmploymentType
	{
		private string employmentTypeName;

		/// <summary>
		/// Gets or sets employment type Id
		/// </summary>
		public int EmploymentTypeId { get; set; }

		/// <summary>
		/// Gets or sets emplyment type's Name
		/// </summary>
		public string EmploymentTypeName
		{
			get => employmentTypeName;
			set
			{
				if (value.Length > 32 || value.Length == 0) throw new ArgumentOutOfRangeException(nameof(employmentTypeName), value, nameof(employmentTypeName) + " must be between 1 and 32 characters in length");
				employmentTypeName = value;
			}
		}
	}
}