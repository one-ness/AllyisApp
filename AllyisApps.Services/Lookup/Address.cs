using System;
using System.Collections.Generic;
using System.Linq;
namespace AllyisApps.Services.Lookup
{
	/// <summary>
	/// An object for keeping track of all info related to a given Address
	/// .
	/// </summary>
	public class Address
	{
		/// <summary>
		/// Gets or sets the address' Id.
		/// </summary>
		public int? AddressId { get; set; }

		/// <summary>
		/// Gets or sets address1.
		/// </summary>
		public string Address1 { get; set; }

		/// <summary>
		/// Gets or sets address2.
		/// </summary>
		public string Address2 { get; set; }

		/// <summary>
		/// Gets or sets the City.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Get or sets the State.
		/// </summary>
		public string StateName { get; set; }

		/// <summary>
		/// Gets or sets the State Id
		/// </summary>
		public int? StateId { get; set; }

		/// <summary>
		/// Gets or sets the PostalCode
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the country
		/// </summary>
		public string CountryName { get; set; }

		/// <summary>
		/// Gets or sets the country code
		/// </summary>
		public string CountryCode { get; set; }

		/// <summary>
		/// Ensures that the object does not have a bad Country Name.
		/// 
		/// Happens mainly in Import Service as it reads in columns based on 
		/// 
		/// TODO: Decide if fix should only happen in import service 
		/// </summary>
		/// <param name="service"></param>
		public void EnsureDBRef(AppService service)
		{
			if (CountryCode == null && CountryName != null)
			{

				try
				{
					CountryCode = service.GetCountries().First(pair => pair.Value.Equals(CountryName)).Key;
				}
				catch (KeyNotFoundException)
				{
					//TODO: Decide to add country to database??
					throw;
				}
			}
			if (StateId == null && StateName != null)
			{
				if (CountryCode == null)
				{
					throw new ArgumentNullException(nameof(CountryCode), "Attempted to add state without country");
				}
				StateId = service
					.GetStates(CountryCode).First(x => (x.Value.Equals(StateName))).Key;
			}
		}
	}
}
