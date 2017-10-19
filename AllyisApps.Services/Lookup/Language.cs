//------------------------------------------------------------------------------
// <copyright file="Language.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.Services.Lookup
{
	/// <summary>
	/// Represents a language setting.
	/// </summary>
	public class Language
	{
		public const string DefaultLanguageCultureName = "en-us";
		public const string DefaultLanguageName = "English (US)";

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
		/// constructor. default language is en-us.
		/// </summary>
		public Language()
		{
			this.LanguageName = DefaultLanguageName;
			this.CultureName = DefaultLanguageCultureName;
		}
	}
}