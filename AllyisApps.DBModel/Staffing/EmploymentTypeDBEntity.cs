namespace AllyisApps.DBModel.Staffing
{
	/// <summary>
	/// Employment Type DB Entity
	/// </summary>
	public class EmploymentTypeDBEntity
	{
		/// <summary>
		/// Gets or sets employment type Id
		/// </summary>
		public int EmploymentTypeId { get; set; }

		/// <summary>
		/// Gets or sets Organization Id
		/// </summary>
		public int OrganizationId { get; set; }

		/// <summary>
		/// Gets or sets emplyment type's Name
		/// </summary>
		public string EmploymentTypeName { get; set; }
	}
}