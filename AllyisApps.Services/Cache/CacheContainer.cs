using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllyisApps.Services.Billing;
using AllyisApps.Services.Auth;
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
		/// dictionary of all skus, indexed by product id
		/// </summary>
		public static Dictionary<ProductIdEnum, List<Sku>> SkusCache { get; private set; }

		/// <summary>
		/// list of all skus in the system
		/// </summary>
		public static List<Sku> AllSkusCache { get; private set; }

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
		public static Dictionary<string, List<State>> StatesCache { get; set; }

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
			StatesCache = new Dictionary<string, List<State>>();
			var stateEntities = DBHelper.GetAllStates();
			foreach (var item in stateEntities)
			{
				var state = new State();
				state.CountryCode = item.Value.CountryCode;
				state.StateId = item.Value.StateId;
				state.StateName = item.Value.StateName;
				// does it exist in cache?
				List<State> list = null;
				if (StatesCache.TryGetValue(state.CountryCode, out list))
				{
					// yes
					list.Add(state);
				}
				else
				{
					// no
					list = new List<State>();
					list.Add(state);
					StatesCache.Add(state.CountryCode, list);
				}
			}

			// init countries
			CountriesCache = new Dictionary<string, Country>();
			var countryEntities = DBHelper.GetCountries();
			foreach (var item in countryEntities)
			{
				var country = new Country();
				country.CountryCode = item.Value.CountryCode;
				country.CountryId = item.Value.CountryId;
				country.CountryName = item.Value.CountryName;
				List<State> states = null;
				if (StatesCache.TryGetValue(country.CountryCode, out states))
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

			// init skus
			SkusCache = new Dictionary<ProductIdEnum, List<Sku>>();
			AllSkusCache = new List<Sku>();
			var skuEntities = DBHelper.GetAllSkus();
			foreach (var item in skuEntities)
			{
				var sku = new Sku();
				sku.BillingFrequency = (BillingFrequencyEnum)item.BillingFrequency;
				sku.UnitSize = item.BlockSize;
				sku.CostPerUnit = item.CostPerBlock;
				sku.IconUrl = item.IconUrl;
				sku.IsActive = item.IsActive;
				sku.ProductId = (ProductIdEnum)item.ProductId;
				sku.PromotionalCostPerUnit = item.PromoCostPerBlock;
				// TODO: need to convert promotion deadline in the database to promotion duration days
				sku.PromotionDurationDays = 0;
				sku.SkuDescription = item.Description;
				sku.SkuId = (SkuIdEnum)item.SkuId;
				sku.SkuName = item.SkuName;
				sku.UnitSize = item.BlockSize;
				sku.UnitType = (UnitTypeEnum)item.BlockBasedOn;
				sku.UserLimit = item.UserLimit;
				List<Sku> list = null;
				// was it added to cache?
				if (SkusCache.TryGetValue(sku.ProductId, out list))
				{
					// yes
					list.Add(sku);
				}
				else
				{
					// no
					list = new List<Sku>();
					list.Add(sku);
					SkusCache.Add(sku.ProductId, list);
				}

				// add to all skus cache as well
				AllSkusCache.Add(sku);
			}

			// init products
			ProductsCache = new Dictionary<ProductIdEnum, Product>();
			var prodEntities = DBHelper.GetProductList();
			foreach (var item in prodEntities)
			{
				var prod = new Product();
				prod.AreaUrl = item.AreaUrl;
				prod.IsActive = item.IsActive;
				prod.ProductDescription = item.Description;
				prod.ProductId = (ProductIdEnum)item.ProductId;
				prod.ProductName = item.ProductName;
				List<Sku> skus = null;
				if (SkusCache.TryGetValue(prod.ProductId, out skus))
				{
					prod.Skus = skus;
				}

				ProductsCache.Add(prod.ProductId, prod);
			}
		}
	}
}
