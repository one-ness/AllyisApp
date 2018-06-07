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
				return connection.Query<LanguageDBEntity>("[Lookup].[GetLanguageById] @a", new { a = CultureName }).FirstOrDefault();
			}
		}

		/// <summary>
		/// Get Address based on addressId
		/// </summary>
		public async Task<AddressDBEntity> GetAddressAsync(int addressID)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return await con.QueryFirstOrDefaultAsync<AddressDBEntity>("[Lookup].[GetAddress] @a", new { a = addressID });
			}
		}

		/// <summary>
		/// create address, returns address id
		/// </summary>
		public async Task<int> CreateAddressAsync(string address1, string address2, string city, int? stateId, string postalCode, string countryCode)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return await con.QueryFirstOrDefaultAsync<int>("[Lookup].[CreateAddress] @p1, @p2, @p3, @p4, @p5, @p6", new { p1 = address1, p2 = address2, p3 = city, p4 = stateId, p5 = postalCode, p6 = countryCode });
			}
		}

		/// <summary>
		/// update the given address, returns number of rows affected
		/// </summary>
		public async Task<int> UpdateAddressAsync(int addressId, string address1, string address2, string city, int? stateId, string postalCode, string countryCode)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return await con.QueryFirstOrDefaultAsync<int>("[Lookup].[UpdateAddress] @p1, @p2, @p3, @p4, @p5, @p6, @p7", new { p1 = addressId, p2 = address1, p3 = address2, p4 = city, p5 = stateId, p6 = postalCode, p7 = countryCode });
			}
		}

		/// <summary>
		/// deletes the given address, returns number of rows affected
		/// </summary>
		public async Task<int> DeleteAddressAsync(int addressId)
		{
			using (var con = new SqlConnection(SqlConnectionString))
			{
				return await con.QueryFirstOrDefaultAsync<int>("[Lookup].[DeleteAddress] @p1", new { p1 = addressId });
			}
		}
	}
}
