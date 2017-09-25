using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AllyisApps.Lib
{
	/// <summary>
	/// Utility methods.
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

		private const int EmailDisplayMaxLength = 48;

		/// <summary>
		/// get a reduced length email if it is too long
		/// </summary>
		public static string GetCompressedEmail(string fullEmail)
		{
			var result = fullEmail;
			if (!string.IsNullOrWhiteSpace(fullEmail) && fullEmail.Length > EmailDisplayMaxLength)
			{
				result = string.Format("{0}...{1}", fullEmail.Substring(0, 20), fullEmail.Substring(fullEmail.Length - 15));
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

		private const string CharsToReplace = @"""/\[]:|<>+=; ,?*'`()@";

		/// <summary>
		/// aggregate spaces in the given string
		/// </summary>
		public static string AggregateSpaces(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return string.Empty;

			return CharsToReplace.Aggregate(input, (str, l) => str.Replace(string.Empty + l, string.Empty));
		}

		/// <summary>
		/// Converts an int representing days since the DateTime min value (Jan 1st, 0001) into a DateTime date.
		/// </summary>
		/// <param name="days">An int of the date as days since Jan 1st, 0001. Use -1 for null date.</param>
		/// <returns>The DateTime date.</returns>
		public static DateTime? GetDateTimeFromDays(int days)
		{
			return DateTime.MinValue.AddDays(days);
		}

		/// <summary>
		/// Converts a DateTime? date into an int representing days since the DateTime min value (Jan 1st, 0001).
		/// </summary>
		/// <param name="date">The DateTime? date.</param>
		/// <returns>An int of the date as days since Jan 1st, 0001. Returns -1 for null.</returns>
		public static int GetDaysFromDateTime(DateTime? date)
		{
			if (!date.HasValue)
			{
				return -1;
			}

			return (int)date.Value.Subtract(DateTime.MinValue).TotalDays;
		}

		/// <summary>
		/// Converts an int representing days since the DateTime min value (Jan 1st, 0001) into a DateTime date.
		/// </summary>
		/// <param name="days">An int of the date as days since Jan 1st, 0001. Use -1 for null dates.</param>
		/// <returns>The DateTime date.</returns>
		public static DateTime? GetDateTimeFromDays(int? days)
		{
			if (!days.HasValue || days <= 0)
			{
				return null;
			}
			return DateTime.MinValue.AddDays(days.Value);
		}
	}
}