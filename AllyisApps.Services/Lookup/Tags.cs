using System.Collections.Generic;

namespace AllyisApps.Services.Lookup
{
	/// <summary>
	/// Represents the Address table in the database.
	/// </summary>
	public class Tags
	{
		/// <summary>
		/// Gets or sets the List of all tags
		/// </summary>
		public IEnumerable<Tag> TagList { get; set; }
	}
}