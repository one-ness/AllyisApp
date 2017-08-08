namespace AllyisApps.DBModel.Lookup
{
	/// <summary>
	/// Country db entity.
	/// </summary>
	public class CountryDBEntity
	{
		/// <summary>
		/// country id.
		/// </summary>
		public int CountryId { get; set; }

		/// <summary>
		/// two character country code.
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// country name.
		/// </summary>
		public string CountryName { get; set; }
	}
}
