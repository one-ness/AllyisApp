namespace AllyisApps.DBModel.Lookup
{
	/// <summary>
	/// Represents the Address table in the database.
	/// </summary>
	public class AddressDBEntity
    {
        /// <summary>
        /// Gets or sets the address' Id
        /// </summary>
        public int? AddressId { get; set; }

        /// <summary>
        /// Gets or sets address1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets address2
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Get or sets the State
        /// </summary>
        public string State { get; set; }

		/// <summary>
		/// Gets or sets the state id
		/// </summary>
		public int? StateId { get; set; }

        /// <summary>
        /// Gets or sets the PostalCode
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public string Country { get; set; }

		/// <summary>
		/// Gets or sets the country id
		/// </summary>
		public int? CountryId { get; set; }

		/// <summary>
		/// Gets or sets the country code
		/// </summary>
		public string CountryCode { get; set; }

    }
}
