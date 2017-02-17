using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
	/// <summary>
	/// Represents the user information serialized to the forms authentication cookie
	/// </summary>
	public class CookieData
	{
		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		public int userId { get; set; }
	}
}
