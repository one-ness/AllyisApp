namespace AllyisApps.DBModel.Lookup
{
	/// <summary>
	/// Country db entity.
	/// </summary>
	public class CountryDBEntity : BaseDBEntity
	{
		/// <summary>
		/// two character country code.
		/// </summary>
		public string CountryCode { get; set; }

		/// <summary>
		/// country name.
		/// </summary>
		public string CountryName { get; set; }
	}
}