using System.Collections.Generic;
using AllyisApps.Services.Billing;
using AllyisApps.DBModel;
using AllyisApps.Services.Lookup;

namespace AllyisApps.Services.Cache
{
	/// <summary>
	/// container for all the caches for all of service layer, used as a singleton
	/// </summary>
	public static class CacheContainer
	{
		/// <summary>
		/// dictionary of all products, indexed by product id
		/// </summary>
		public static Dictionary<ProductIdEnum, Product> ProductsCache { get; private set; }

		/// <summary>
		/// dictionary of all langugages, indexed by culture name
		/// </summary>
		public static Dictionary<string, Language> LanguagesCache { get; set; }

		/// <summary>
		/// dictionary of all countries, indexed by country code
		/// </summary>
		public static Dictionary<string, Country> CountriesCache { get; set; }

		/// <summary>
		/// dictionary of all states, indexed by country code
		/// </summary>
		public static Dictionary<string, List<State>> StatesForCountryCache { get; set; }

		/// <summary>
		/// dictionary of all states, indexed by state id
		/// </summary>
		public static Dictionary<int, State> AllStatesCache { get; set; }

		private static string sqlConnectionString;
		private static DBHelper DBHelper;

		/// <summary>
		/// init
		/// </summary>
		public static void Init(string connectionString)
		{
			sqlConnectionString = connectionString;
			DBHelper = new DBHelper(sqlConnectionString);

			// init states
			StatesForCountryCache = new Dictionary<string, List<State>>();
			AllStatesCache = new Dictionary<int, State>();
			var stateEntities = DBHelper.GetAllStates();
			foreach (var item in stateEntities)
			{
				var state = new State();
				state.CountryCode = item.Value.CountryCode;
				state.StateId = item.Value.StateId;
				state.StateName = item.Value.StateName;
				// does it exist in cache?
				List<State> list = null;
				if (StatesForCountryCache.TryGetValue(state.CountryCode, out list))
				{
					// yes
					list.Add(state);
				}
				else
				{
					// no
					list = new List<State>();
					list.Add(state);
					StatesForCountryCache.Add(state.CountryCode, list);
				}

				// add it to all states cache
				AllStatesCache.Add(state.StateId, state);
			}

			// init countries
			CountriesCache = new Dictionary<string, Country>();
			var countryEntities = DBHelper.GetCountries();
			foreach (var item in countryEntities)
			{
				var country = new Country();
				country.CountryCode = item.Value.CountryCode;
				country.CountryName = item.Value.CountryName;
				List<State> states = null;
				if (StatesForCountryCache.TryGetValue(country.CountryCode, out states))
				{
					country.States = states;
				}

				CountriesCache.Add(country.CountryCode, country);
			}

			// init languages
			LanguagesCache = new Dictionary<string, Language>();
			var langEntities = DBHelper.GetLanguages();
			foreach (var item in langEntities)
			{
				var lang = new Language();
				lang.CultureName = item.Value.CultureName;
				lang.LanguageName = item.Value.LanguageName;
				LanguagesCache.Add(lang.CultureName, lang);
			}

			// init products
			ProductsCache = new Dictionary<ProductIdEnum, Product>();
			var prodEntities = DBHelper.GetProductList();
			foreach (var item in prodEntities)
			{
				var prod = new Product();
				prod.AreaUrl = item.AreaUrl;
                prod.ProductStatus = (item.ProductStatus == 1) ? 
                    ProductStatusEnum.Active : ProductStatusEnum.Inactive;
				prod.ProductDescription = item.Description;
				prod.ProductId = (ProductIdEnum)item.ProductId;
				prod.ProductName = item.ProductName;
				ProductsCache.Add(prod.ProductId, prod);
			}
		}
	}
}
