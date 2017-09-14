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
using AllyisApps.DBModel.Lookup;
using Dapper;

namespace AllyisApps.DBModel
{
	/// <summary>
	/// DBHelper Partial.
	/// </summary>
	public partial class DBHelper
	{
		/// <summary>
		/// list of valid countries
		/// </summary>
		public Dictionary<string, string> GetCountries()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<CountryDBEntity>("[Lookup].[GetCountries]").ToDictionary(x => x.CountryCode, x => x.CountryName);
			}
		}

		/// <summary>
		/// list of states for the given country
		/// </summary>
		public Dictionary<int, string> GetStates(string countryCode)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<StateDBEntity>("[Lookup].[GetStates] @a", new { a = countryCode }).ToDictionary(x => x.StateId, x => x.StateName);
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
		/// <param name="CultureName">The language Id.</param>
		/// <returns>A language setting row.</returns>
		public LanguageDBEntity GetLanguage(string CultureName)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<LanguageDBEntity>("[Lookup].[GetLanguageById]", new { CultureName = CultureName }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Get Address based on addres ID
		/// </summary>
		/// <param name="addressID"></param>
		/// <returns></returns>
		public AddressDBEntity getAddreess(int? addressID)
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<AddressDBEntity>("[Lookup].[GetAddress]", new { addresId = addressID }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}
	}
}