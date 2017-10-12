using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			this.States = new List<State>();
		}
	}
}
