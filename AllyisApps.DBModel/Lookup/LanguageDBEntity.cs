//------------------------------------------------------------------------------
// <copyright file="LanguageDBEntity.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AllyisApps.DBModel.Lookup
{
	/// <summary>
	/// The model for the language table.
	/// </summary>
	public class LanguageDBEntity
	{
		/// <summary>
		/// Gets or sets the language Id.
		/// </summary>
		public int LanguageId { get; set; }

		/// <summary>
		/// Gets or sets the language name.
		/// </summary>
		public string LanguageName { get; set; }

		/// <summary>
		/// Gets or sets the culture name string, as used to construct a CultureInfo instance (e.g. "en-US").
		/// See https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.aspx for more info.
		/// </summary>
		public string CultureName { get; set; }
	}
}
