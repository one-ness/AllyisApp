using System.Collections.Generic;

namespace AllyisApps.Services.Lookup
{
	/// <summary>
	/// country
	/// </summary>
	public class Country
	{
		public int CountryId { get; set; }
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
		public List<State> States { get; set; }
		
		/// <summary>
		/// constructor
		/// </summary>
		public Country()
		{
			States = new List<State>();
		}
	}
}
