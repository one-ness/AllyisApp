//------------------------------------------------------------------------------
// <copyright file="DBHelper.Lookup.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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
		public Dictionary<string, CountryDBEntity> GetCountries()
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return con.Query<CountryDBEntity>("[Lookup].[GetCountries]").ToDictionary(x => x.CountryCode, x => x);
			}
		}

		/// <summary>
		/// get all states
		/// </summary>
		public Dictionary<int, StateDBEntity> GetAllStates()
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return con.Query<StateDBEntity>("Lookup.GetAllStates").ToDictionary(x => x.StateId, x => x);
			}
		}

		/// <summary>
		/// list of states for the given country
		/// </summary>
		public Dictionary<int, string> GetStates(string countryCode)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return con.Query<StateDBEntity>("[Lookup].[GetStates] @a", new { a = countryCode }).ToDictionary(x => x.StateId, x => x.StateName);
			}
		}

		/// <summary>
		/// get a list of all languages
		/// </summary>
		public Dictionary<string, LanguageDBEntity> GetLanguages()
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return con.Query<LanguageDBEntity>("Lookup.GetLanguages").ToDictionary(x => x.CultureName, x => x);
			}
		}

		/// <summary>
		/// Retrieves a collection of language settings from the database.
		/// </summary>
		public async Task<List<LanguageDBEntity>> ValidLanguages()
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				var result = await con.QueryAsync<LanguageDBEntity>("[Lookup].[GetLanguages]");
				return result.ToList();
			}
		}

		/// <summary>
		/// Retrieves a language setting from the database.
		/// </summary>
		/// <param name="CultureName">The language Id.</param>
		/// <returns>A language setting row.</returns>
		public LanguageDBEntity GetLanguage(string CultureName)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<LanguageDBEntity>("[Lookup].[GetLanguageById]", new { CultureName = CultureName }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}

		/// <summary>
		/// Get Address based on addres ID
		/// </summary>
		public AddressDBEntity GetAddreess(int addressID)
		{
			using (SqlConnection connection = new SqlConnection(SqlConnectionString))
			{
				return connection.Query<AddressDBEntity>("[Lookup].[GetAddress]", new { addresId = addressID }, commandType: CommandType.StoredProcedure).SingleOrDefault();
			}
		}
	}
}