//------------------------------------------------------------------------------
// <copyright file="DBHelper.Lookup.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AllyisApps.DBModel.Shared;
using Dapper;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// Retrieves a collection of valid countries from the database.
		/// </summary>
		/// <returns>A collection of country names.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called through Razor engine.")]
		public IEnumerable<string> ValidCountries()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Lookup].[GetCountries]", commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves a collection of valid states (or provinces)
		///     from the database based on the source country.
		/// </summary>
		/// <param name="countryName">The country's name.</param>
		/// <returns>A collection of states/provinces within that country.</returns>
		public IEnumerable ValidStates(string countryName)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<string>("[Lookup].[GetStatesByCountry]", new { CountryName = countryName }, commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves a collection of language settings from the database.
		/// </summary>
		/// <returns>A collection of all valid language settings.</returns>
		public IEnumerable<LanguageDBEntity> ValidLanguages()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<LanguageDBEntity>("[Lookup].[GetLanguages]", commandType: CommandType.StoredProcedure);
			}
		}

		/// <summary>
		/// Retrieves a language setting from the database.
		/// </summary>
		/// <param name="languageID">The language ID.</param>
		/// <returns>A language setting row.</returns>
		public LanguageDBEntity GetLanguage(int languageID)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<LanguageDBEntity>("[Lookup].[GetLanguageByID]", new { LanguageID = languageID }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}
	}
}