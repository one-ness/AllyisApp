using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.DBModel.Lookup
{
	/// <summary>
	/// country db entity
	/// </summary>
	public class CountryDBEntity
	{
		/// <summary>
		/// country id
		/// </summary>
		public int CountryId { get; set; }

		/// <summary>
		/// two character country code
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// country name
		/// </summary>
		public string Name { get; set; }
	}
}
