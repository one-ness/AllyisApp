namespace AllyisApps.DBModel.StaffingManager
{
	/// <summary>
	/// Represents the Address table in the database.
	/// </summary>
	public class TagDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the Tags Id
		/// </summary>
		public int TagId { get; set; }

		/// <summary>
		/// Gets or sets Position Id
		/// </summary>
		public int PositionId { get; set; }

		/// <summary>
		/// Gets or sets the Tag's Name
		/// </summary>
		public string TagName { get; set; }

	}
}
