namespace AllyisApps.Areas.StaffingManager.ViewModels.Staffing
{
	/// <summary>
	/// Position Level select View Model
	/// </summary>
	public class PositionLevelSelectViewModel
	{
		/// <summary>
		/// Position Level id.
		/// </summary>
		public int PositionLevelId { get; set; }

		/// <summary>
		/// Position Level Status name.
		/// </summary>
		public string PositionLevelName { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class PositionStatusSelectViewModel
	{
		/// <summary>
		/// Position Status Id
		/// </summary>
		public int PositionStatusId { get; set; }

		/// <summary>
		/// Position Status Name
		/// </summary>
		public string PositionStatusName { get; set; }
	}

	/// <summary>
	/// Empolyment types.
	/// </summary>
	public class EmploymentTypeSelectViewModel
	{
		/// <summary>
		/// Employment type identifier
		/// </summary>
		public int EmploymentTypeId { get; set; }

		/// <summary>
		/// Empolyent type Name
		/// </summary>
		public string EmploymentTypeName { get; set; }
	}

	/// <summary>
	/// Customer drop down.
	/// </summary>
	public class CustomerSelectViewModel
	{
		/// <summary>
		/// Gets or sets identificer for customer.
		/// </summary>
		public int CustomerId { get; set; }

		/// <summary>
		/// Gets for sets for Customer's Name
		/// </summary>
		public string CustomerName { get; set; }
	}
}