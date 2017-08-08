//------------------------------------------------------------------------------
// <copyright file="DBHelper.Lookup.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel.Lookup;
using Dapper;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
		public Dictionary<int, string> GetCountriesDictionary()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<CountryDBEntity>("[Lookup].[GetCountries]").ToDictionary(x => x.CountryId, x => x.CountryName);
			}
		}

		/// <summary>
		/// list of valid countries.
		/// </summary>
		public List<string> ValidCountries()
		{
			using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
			{
				return connection.Query<CountryDBEntity>("[Lookup].[GetCountries]").Select(x => x.CountryName).ToList();
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
        /// Retrieve a address from the database via an address id.
        /// </summary>
        /// <param name="addressID"></param>
        /// <returns></returns>
        public AddressDBEntity GetAddressByAddressId(int addressID)
        {
            using (SqlConnection connection = new SqlConnection(this.SqlConnectionString))
            {
                return connection.Query<AddressDBEntity>("[Lookup].[GetAddress]", new { addressID = addressID }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
        }
	}
}
