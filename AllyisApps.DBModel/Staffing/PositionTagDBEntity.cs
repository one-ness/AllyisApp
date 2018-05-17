namespace AllyisApps.DBModel.Staffing
{
	/// <summary>
	/// Represents the Address table in the database.
	/// </summary>
	public class PositionTagDBEntity : BaseDBEntity
	{
		/// <summary>
		/// Gets or sets the Tags Id
		/// </summary>
		public int TagId { get; set; }

		/// <summary>
		/// Gets or sets the Tag's Name
		/// </summary>
		public string TagName { get; set; }

		/// <summary>
		/// Gets or sets the Position Id
		/// </summary>
		public int PositionId { get; set; }
	}
}