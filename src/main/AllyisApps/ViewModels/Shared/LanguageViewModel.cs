//------------------------------------------------------------------------------
// <copyright file="LanguageViewModel.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Linq;

namespace AllyisApps.ViewModels
{
	/// <summary>
	/// View model for a language setting.
	/// </summary>
	public class LanguageViewModel
	{
		private const string CharsToReplace = @"""/\[]:|<>+=; ,?*'`()@";

		/// <summary>
		/// Gets or sets the language ID.
		/// </summary>
		public int LanguageID { get; set; }

		/// <summary>
		/// Gets or sets the language name.
		/// </summary>
		public string LanguageName { get; set; }

		/// <summary>
		/// Gets or sets the culture name string, as used to construct a CultureInfo instance (e.g. "en-US").
		/// See https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.aspx for more info.
		/// </summary>
		public string CultureName { get; set; }

		/// <summary>
		/// Cleans languageName for keying.
		/// </summary>
		/// <param name="stringToClean">The string to clean.</param>
		/// <returns>Key version of language name.</returns>
		public static string Clean(string stringToClean)
		{
			return CharsToReplace.Aggregate(stringToClean, (str, l) => str.Replace(string.Empty + l, string.Empty));
		}
	}
}