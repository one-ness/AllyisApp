using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AllyisApps.Lib
{
	/// <summary>
	/// utility methods
	/// </summary>
	public static class Utility
	{
		/// <summary>
		/// Verifies an email address format.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <returns>True if it is a valid format, false if not.</returns>
		public static bool IsValidEmail(string email)
		{
			bool result = false;
			if (!string.IsNullOrWhiteSpace(email))
			{
				result = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            }
			return result;
		}

		/// <summary>
		/// Verifies a url format for a web address (http or https).
		/// </summary>
		/// <param name="url">The url.</param>
		/// <returns>True if it is a valid web url, false if not.</returns>
		public static bool IsValidUrl(string url)
		{
			Uri result;
			return Uri.TryCreate(url, UriKind.Absolute, out result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
		}
	}
}
