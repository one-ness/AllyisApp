using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.Lookup
{
	/// <summary>
	/// state db entity
	/// </summary>
	public class StateDBEntity
	{
		/// <summary>
		/// state id
		/// </summary>
		public int StateId { get; set; }

		/// <summary>
		/// country id
		/// </summary>
		public int CountryId { get; set; }

		/// <summary>
		/// state name
		/// </summary>
		public string StateName { get; set; }
	}
}
